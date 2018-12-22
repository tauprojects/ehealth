package ehealth.service;

import ehealth.client.StrainServicesInterface;
import ehealth.data_objects.LoginRequest;
import ehealth.client.data_objects.Strain;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.RegisterRequest;
import ehealth.exceptions.BadRequestException;
import ehealth.identity.Identity;
import ehealth.service.api.StrainApiService;
import org.jboss.logging.Logger;
import org.jboss.resteasy.client.jaxrs.ResteasyClient;
import org.jboss.resteasy.client.jaxrs.ResteasyClientBuilder;
import org.jboss.resteasy.client.jaxrs.ResteasyWebTarget;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.ws.rs.core.UriBuilder;
import java.util.List;
import java.util.UUID;

import static ehealth.client.ApiConstants.URL;


@Service
public class StrainApiServiceImpl implements StrainApiService {

    protected ResteasyClient client;
    protected ResteasyWebTarget target;
    protected StrainServicesInterface restClient;
    Logger logger = Logger.getLogger(StrainApiServiceImpl.class);

    @Autowired
    protected Identity identity;


    public StrainApiServiceImpl() {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
    }

    @Override
    public BaseResponse getStrainByName(String strainName) {
        List<Strain> strainResponse = restClient.strainByName(strainName);
        System.out.println("HTTP code: " + strainResponse.get(0).toString());
        BaseResponse resp = new BaseResponse(
                UUID.randomUUID(),
                "Exist",
                strainResponse.get(0).getDesc());
        return resp;
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
    public BaseResponse authenticate(LoginRequest loginRequest) {
        BaseResponse resp;
        String user = loginRequest.getUsername();
        String password = loginRequest.getPassword();
        if (identity.users.get(user) == null) {
            throw new BadRequestException();
        }
        if (!identity.users.get(user).equals(password)) {
            throw new BadRequestException();
        }
        return new BaseResponse(null, "OK", "User: " + user +" Authenticated successfully");
    }

    @Override
    public BaseResponse register(RegisterRequest registerRequest) {
        if (registerRequest != null){
            logger.info(registerRequest.toString());
        }
        return new BaseResponse(null,"CREATED","User created");
    }
}
