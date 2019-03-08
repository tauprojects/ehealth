package ehealth.service;

import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.Strain;
import ehealth.client.data_objects.StrainObject;
import ehealth.data_objects.*;
import ehealth.db.model.RegisteredUsersEntity;
import ehealth.db.model.UsageHistoryEntity;
import ehealth.db.repository.RegisterUsersRepository;
import ehealth.exceptions.BadRegisterRequestException;
import ehealth.exceptions.BadRequestException;
import ehealth.service.api.StrainApiService;
import org.jboss.resteasy.client.jaxrs.ResteasyClient;
import org.jboss.resteasy.client.jaxrs.ResteasyClientBuilder;
import org.jboss.resteasy.client.jaxrs.ResteasyWebTarget;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.ws.rs.core.UriBuilder;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.UUID;

import static ehealth.client.ApiConstants.URL;


@Service
public class StrainApiServiceImpl implements StrainApiService {

    protected ResteasyClient client;
    protected ResteasyWebTarget target;
    protected StrainServicesInterface restClient;
    private Logger logger = LoggerFactory.getLogger(StrainApiServiceImpl.class);

    @Autowired
    protected RegisterUsersRepository registerUsersRepository;

    @Autowired
    protected StrainsCollector strainsCollector;

    public StrainApiServiceImpl() {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
    }

    @Override
    public BaseResponse getStrainByName(String strainName) {
        List<Strain> strainResponse = restClient.strainByName(strainName);
        System.out.println("HTTP code: " + strainResponse.get(0).toString());
//        BaseResponse resp = new BaseResponse(
//                UUID.randomUUID(),
//                "Exist",
//                strainResponse.get(0).getDesc());
        return null;
    }


    @Override
    public RegisteredUserData authenticate(LoginRequest loginRequest) {
        BaseResponse resp;
        String user = loginRequest.getUsername();
        String password = loginRequest.getPassword();
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findByUsername(loginRequest.getUsername());
        if (registeredUsersEntity == null) {
            logger.error("Username: " + user + " does not exist in DB");
            throw new BadRequestException();
        }
        if (!registeredUsersEntity.getPassword().equals(password)) {
            logger.error("Bad password for user: " + user);
            throw new BadRequestException();
        }
        logger.info("User: " + user + " Authenticated successfully");
        // return registered user data
        return createUserDataResponseFromEntity(registeredUsersEntity);
    }


    @Override
    public RegisteredUserData register(RegisterRequest registerRequest) {
        if (registerRequest != null) {
            logger.info(registerRequest.toString());
            RegisteredUsersEntity registeredUsersEntity = new RegisteredUsersEntity();
            // Generate Unique User Id
            registeredUsersEntity.setId(UUID.randomUUID());
            // Set createdAt as current time
            registeredUsersEntity.setCreatedAt(System.currentTimeMillis());
            // Set input user-data
            registeredUsersEntity.setUsername(registerRequest.getUsername());
            registeredUsersEntity.setPassword(registerRequest.getPassword());
            registeredUsersEntity.setCity(registerRequest.getCity());
            registeredUsersEntity.setCountry(registerRequest.getCountry());
            registeredUsersEntity.setDob(registerRequest.getDOB());
            registeredUsersEntity.setGender(registerRequest.getGender());
            registeredUsersEntity.setMedical(registerRequest.getMedical());
            registeredUsersEntity.setPositive(registerRequest.getPositive());
            registeredUsersEntity.setNegative(registerRequest.getNegative());
            // Save to DB
            registerUsersRepository.save(registeredUsersEntity);
            return createUserDataResponseFromEntity(registeredUsersEntity);
        }
        throw new BadRegisterRequestException();
    }

    @Override
    public List<StrainObject> getRecommendedStrain(String userId) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));
        int medical = registeredUsersEntity.getMedical();
        int positive = registeredUsersEntity.getPositive();

        List<StrainObject> recommendedStrains = new ArrayList<>();
        for (StrainObject strain : strainsCollector.allStrains) {
            int medicalCand = strain.getMedical().intValue();
            int positiveCand = strain.getPositive().intValue();
            if ((medicalCand & medical) == medical && (positiveCand & positive) == positive) {
                // Add only if strain is not blacklisted
                if(!registeredUsersEntity.getBlacklist().contains(strain.getId())) {
                    recommendedStrains.add(strain);
                }
            }
        }

        return recommendedStrains;
    }


    @Override
    public void saveUsageHistoryForUser(UsageHistory usageHistory) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(usageHistory.getUserId()));
        if(registeredUsersEntity==null){
            throw new BadRequestException();
        }
        UsageHistoryEntity usageHistoryEntity = buildUsageHistoryEntity(usageHistory);
        List<UsageHistoryEntity> usageHistoryEntityList;
        if (registeredUsersEntity.getUsageHistoryEntity() == null) {
            usageHistoryEntityList = Arrays.asList(usageHistoryEntity);
        } else {
            usageHistoryEntityList = registeredUsersEntity.getUsageHistoryEntity();
            usageHistoryEntityList.add(usageHistoryEntity);
        }
        registeredUsersEntity.setUsageHistoryEntity(usageHistoryEntityList);
        // Update blackList
        if(usageHistory.getIsBlacklist()==1){
            List<Integer> blacklist = registeredUsersEntity.getBlacklist();
            blacklist.add(usageHistoryEntity.getStrainId());
            registeredUsersEntity.setBlacklist(blacklist);
            logger.info("String Id: " + usageHistoryEntity.getStrainId().toString() + " added to blacklist");
        }
        registerUsersRepository.save(registeredUsersEntity);
    }

    @Override
    public List<UsageHistoryResponse> getUsageHistoryForUser(String userId) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));
        List<UsageHistoryResponse> usageHistoryResponseList = new ArrayList<>();
        for (UsageHistoryEntity usageHistoryEntity : registeredUsersEntity.getUsageHistoryEntity()) {
            usageHistoryResponseList.add(new UsageHistoryResponse(
                    usageHistoryEntity.getId(),
                    usageHistoryEntity.getUserId(),
                    usageHistoryEntity.getStartedAt(),
                    usageHistoryEntity.getEndedAt(),
                    usageHistoryEntity.getStrainName(),
                    usageHistoryEntity.getStrainId(),
                    usageHistoryEntity.getMedicalRank(),
                    usageHistoryEntity.getPositiveRank(),
                    usageHistoryEntity.getOverallRank(),
                    usageHistoryEntity.getHeartbeatHigh(),
                    usageHistoryEntity.getHeartbeatLow(),
                    usageHistoryEntity.getHeartbeatAvg()
            ));
        }
        return usageHistoryResponseList;
    }

    @Override
    public List<StrainObject> getAllStrains() throws IOException {
        return strainsCollector.allStrains;
    }

    private UsageHistoryEntity buildUsageHistoryEntity(UsageHistory usageHistory) {
        UsageHistoryEntity usageHistoryEntity = new UsageHistoryEntity();
        usageHistoryEntity.setId(UUID.randomUUID());
        usageHistoryEntity.setUserId(UUID.fromString(usageHistory.getUserId()));
        usageHistoryEntity.setStartedAt(usageHistory.getStartTime());
        usageHistoryEntity.setEndedAt(usageHistory.getEndTime());
        usageHistoryEntity.setStrainId(usageHistory.getStrainId());
        usageHistoryEntity.setStrainName(usageHistory.getStrainName());
        usageHistoryEntity.setHeartbeatHigh(usageHistory.getHeartbeatHigh());
        usageHistoryEntity.setHeartbeatAvg(usageHistory.getHeartbeatAvg());
        usageHistoryEntity.setHeartbeatLow(usageHistory.getHeartbeatLow());
        usageHistoryEntity.setMedicalRank(usageHistory.getMedicalRank());
        usageHistoryEntity.setPositiveRank(usageHistory.getPositiveRank());
        usageHistoryEntity.setOverallRank(usageHistory.getOverallRank());
        return usageHistoryEntity;
    }

    private RegisteredUserData createUserDataResponseFromEntity(RegisteredUsersEntity registeredUsersEntity) {
        return new RegisteredUserData(
                registeredUsersEntity.getId(),
                registeredUsersEntity.getUsername(),
                registeredUsersEntity.getDob(),
                registeredUsersEntity.getGender(),
                registeredUsersEntity.getCountry(),
                registeredUsersEntity.getCity(),
                registeredUsersEntity.getMedical(),
                registeredUsersEntity.getPositive(),
                registeredUsersEntity.getNegative(),
                registeredUsersEntity.getCreatedAt()
        );
    }
}
