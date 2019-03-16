package ehealth.service;

import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.StrainObject;
import ehealth.client.data_objects.SuggestedStrains;
import ehealth.data_objects.*;
import ehealth.db.model.RegisteredUsersEntity;
import ehealth.db.model.StrainsEntity;
import ehealth.db.model.UsageHistoryEntity;
import ehealth.db.repository.AllStrainsRepository;
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
import java.util.*;

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
    StrainsCollector strainsCollector;

    @Autowired
    protected EmailService emailService;

    @Autowired
    protected AllStrainsRepository allStrainsRepository;

    public StrainApiServiceImpl() {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
    }

    @Override
    public RegisteredUserData authenticate(LoginRequest loginRequest) {
        BaseResponse resp;
        String user = loginRequest.getUsername().toLowerCase();
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
            logger.info("Register request: " + registerRequest.toString());
            RegisteredUsersEntity registeredUsersEntity = new RegisteredUsersEntity();
            // Generate Unique User Id
            registeredUsersEntity.setId(UUID.randomUUID());
            // Set createdAt as current time
            registeredUsersEntity.setCreatedAt(System.currentTimeMillis());
            // Set input user-data
            registeredUsersEntity.setUsername(registerRequest.getUsername().toLowerCase());
            registeredUsersEntity.setPassword(registerRequest.getPassword());
            registeredUsersEntity.setCity(registerRequest.getCity());
            registeredUsersEntity.setEmail(registerRequest.getEmail());
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
    public RegisteredUserData edit(String userId, RegisterRequest registerRequest) {
        if (registerRequest != null) {

            logger.info("Edit request: " + registerRequest.toString());
            RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));            // Generate Unique User Id
            // Set input user-data
            if (registerRequest.getMedical() != registeredUsersEntity.getMedical()) {
                registeredUsersEntity.setMedical(registerRequest.getMedical());
            }
            if (registerRequest.getPositive() != registeredUsersEntity.getPositive()) {
                registeredUsersEntity.setPositive(registerRequest.getPositive());
            }
            if (registerRequest.getNegative() != registeredUsersEntity.getNegative()) {
                registeredUsersEntity.setNegative(registerRequest.getNegative());
            }
            // Save to DB
            registerUsersRepository.save(registeredUsersEntity);
            return createUserDataResponseFromEntity(registeredUsersEntity);
        }
        throw new BadRegisterRequestException();
    }

    @Override
    public StrainObject getStrainByName(String strainName) {
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainName(strainName);
        if (strainsEntity == null) {
            throw new BadRequestException();
        }
        return strainEntityToStrainObject(strainsEntity);
    }

    @Override
    public StrainObject getStrainById(Integer strainId) {
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainId(strainId);
        if (strainsEntity == null) {
            throw new BadRequestException();
        }
        return strainEntityToStrainObject(strainsEntity);
    }

    @Override
    public List<StrainObject> getAllStrains() throws IOException {
        List<StrainObject> allStrains = new ArrayList<>();
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();
        for (StrainsEntity strain : strainsEntities) {
            allStrains.add(strainEntityToStrainObject(strain));
        }
        return allStrains;
    }

    @Override
    public Map<String, Integer> GetListOfStrains() {
        Map<String, Integer> listOfStrains = new HashMap<>();
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();
        for (StrainsEntity strain : strainsEntities) {
            listOfStrains.put(strain.getStrainName(), strain.getStrainId());
        }
        return listOfStrains;
    }


    @Override
    public SuggestedStrains getRecommendedStrain(String userId) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));
        int medical = registeredUsersEntity.getMedical();
        int positive = registeredUsersEntity.getPositive();
        SuggestedStrains suggestedStrains = getStrainByEffects(medical, positive);
        List<StrainObject> listOfSuggestedStrains = suggestedStrains.getSuggestedStrains();

        // Filter blacklisted strains
        for (int i = 0; i < listOfSuggestedStrains.size(); i++)
            if (!registeredUsersEntity.getBlacklist().contains(listOfSuggestedStrains.get(i).getId())) {
                listOfSuggestedStrains.remove(i);
            }
        return suggestedStrains;
    }

    @Override
    public SuggestedStrains getStrainByEffects(int medical, int positive) {
        SuggestedStrains recommendedStrains = new SuggestedStrains();

        // Check for strains that fit exact medical and positive effects
        List<StrainsEntity> strainsEntities = allStrainsRepository.findAll();

        for (StrainsEntity strain : strainsEntities) {
            int medicalCand = strain.getMedical();
            int positiveCand = strain.getPositive();
            if ((medicalCand & medical) == medical && (positiveCand & positive) == positive) {
                recommendedStrains.addStrain(strainEntityToStrainObject(strain));
            }
        }
        // Check for strains that fit exact medical effects
        // Check for strains that similar to positive by number of effects
        if (recommendedStrains.getSuggestedStrains().size() == 0) {
            logger.info("Did not fine any strain that fits preferences");
            for (int i = 1; i < CountBits(positive); i++) {
                logger.info("Search for similar positive strains that differ in : " + i + " number of effects");
                for (StrainsEntity strain : strainsEntities) {
                    int medicalCand = strain.getMedical();
                    int positiveCand = strain.getPositive();
                    if ((medicalCand & medical) == medical &&
                            (CountBits(positiveCand ^ positive)) == i) {
                        recommendedStrains.addStrain(strainEntityToStrainObject(strain));
                    }
                }
                if (recommendedStrains.getSuggestedStrains().size() > 0) {
                    recommendedStrains.setStatus(1);
                    break;
                }
            }
        }
        // Check for strains that fit only medical effects - similar
        if (recommendedStrains.getSuggestedStrains().size() == 0) {
            logger.info("Did not fine any strain that fits preferences with similar positive");
            for (int i = 0; i < CountBits(medical); i++) {
                logger.info("Search for similar medical strains that differ in: " + i + " number of effects");
                for (StrainsEntity strain : strainsEntities) {
                    int medicalCand = strain.getMedical();
                    if ((CountBits(medicalCand ^ medical)) == i) {
                        recommendedStrains.addStrain(strainEntityToStrainObject(strain));
                    }
                }
                if (recommendedStrains.getSuggestedStrains().size() > 0) {
                    recommendedStrains.setStatus(2);
                    break;
                }
            }
        }
        return recommendedStrains;

    }

    @Override
    public void saveUsageHistoryForUser(UsageHistory usageHistory) {
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(usageHistory.getUserId()));
        if (registeredUsersEntity == null) {
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
        if (usageHistory.getIsBlacklist() == 1) {
            List<Integer> blacklist = registeredUsersEntity.getBlacklist();
            blacklist.add(usageHistoryEntity.getStrainId());
            registeredUsersEntity.setBlacklist(blacklist);
            logger.info("String Id: " + usageHistoryEntity.getStrainId().toString() + " added to blacklist");
        }

        registerUsersRepository.save(registeredUsersEntity);

        // Update strain rank
        StrainsEntity strainsEntity = allStrainsRepository.findByStrainId(usageHistory.getStrainId());
        int numOfUsages = strainsEntity.getNumberOfUsages();
        double rank = strainsEntity.getRank();
        strainsEntity.setRank((numOfUsages * rank + usageHistory.getOverallRank()) / (numOfUsages + 1));
        strainsEntity.setNumberOfUsages(numOfUsages + 1);
        allStrainsRepository.save(strainsEntity);
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
                    usageHistoryEntity.getHeartbeatAvg(),
                    usageHistoryEntity.getQuestionsAnswersDictionary()));
        }
        return usageHistoryResponseList;
    }

    @Override
    public BaseResponse exportToEmail(String userId, String to, String userContent) throws IOException {
        BaseResponse resp = new BaseResponse();
        RegisteredUsersEntity registeredUsersEntity = registerUsersRepository.findById(UUID.fromString(userId));            // Generate Unique User Id
        if (registeredUsersEntity == null) {
            throw new BadRequestException();
        }
        List<UsageHistoryResponse> usageHistoryEntityList = getUsageHistoryForUser(userId);
        String subject = "Usage History for:  " + registeredUsersEntity.getUsername();
        StringBuilder content = new StringBuilder();
        content.append("This is an email from Medicanna app.   ").append("\n");
        content.append("Description: ").append(userContent).append("\n");
        for (UsageHistoryResponse usageHistoryResponse : usageHistoryEntityList) {
            content.append(usageHistoryResponse.toString()).append("\r\n");
        }
        int emailResp = emailService.sendEmail(registeredUsersEntity.getUsername(), to, subject, content.toString());
        resp.setBody("Usage history exported to: " + to + " successfully");
        resp.setStatus(String.valueOf(emailResp));
        return resp;
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
        usageHistoryEntity.setQuestionsAnswersDictionary(usageHistory.getQuestionsAnswersDictionary());
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
                registeredUsersEntity.getEmail(),
                registeredUsersEntity.getMedical(),
                registeredUsersEntity.getPositive(),
                registeredUsersEntity.getNegative(),
                registeredUsersEntity.getCreatedAt()
        );
    }

    private StrainObject strainEntityToStrainObject(StrainsEntity strainsEntity) {
        StrainObject strainObject = new StrainObject();
        strainObject.setDescription(strainsEntity.getDescription());
        strainObject.setName(strainsEntity.getStrainName());
        strainObject.setId(strainsEntity.getStrainId());
        strainObject.setMedical(new Long(strainsEntity.getMedical()));
        strainObject.setPositive(new Long(strainsEntity.getPositive()));
        strainObject.setNegative(new Long(strainsEntity.getNegative()));
        strainObject.setRace(strainsEntity.getRace());
        strainObject.setRank(strainsEntity.getRank());
        strainObject.setNumberOfUsages(strainsEntity.getNumberOfUsages());
        return strainObject;
    }

    private static int CountBits(int num) {
        int tmpNum = num, count = 0;
        while (tmpNum > 0) {
            count += tmpNum & 1;
            tmpNum >>= 1;
        }
        return count;
    }
}
