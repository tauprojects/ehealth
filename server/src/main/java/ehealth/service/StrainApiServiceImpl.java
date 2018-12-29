package ehealth.service;

import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.Strain;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.LoginRequest;
import ehealth.data_objects.RegisterRequest;
import ehealth.data_objects.RegisteredUserData;
import ehealth.db.model.RegisteredUsersEntity;
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
import java.util.ArrayList;
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
    public String getStrainByEffect(String effectName) {
        return null;
    }

    @Override
    public String getStrainById(String strainId) {
        return null;
    }

    @Override
    public String getAllEffects(String strainName) {
        return null;
    }

    @Override
    public String getAllStrains(String strainName) {
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

    private RegisteredUserData createUserDataResponseFromEntity(RegisteredUsersEntity registeredUsersEntity) {
        return new RegisteredUserData(
                registeredUsersEntity.getId(),
                registeredUsersEntity.getUsername(),
                registeredUsersEntity.getDob(),
                registeredUsersEntity.getGender(),
                registeredUsersEntity.getCountry(),
                registeredUsersEntity.getCity(),
                getEffectsAsString(registeredUsersEntity.getMedical()),
                getEffectsAsString(registeredUsersEntity.getPositive()),
                getEffectsAsString(registeredUsersEntity.getNegative()),
                registeredUsersEntity.getCreatedAt()
        );
    }

    private List<String> getEffectsAsString(List<?> effectsList) {
        List<String> effectStringList = new ArrayList<>();
        for (Object effect : effectsList) {
            effectStringList.add(effect.toString());
        }
        return effectStringList;

    }
}
