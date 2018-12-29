package ehealth.service.api;

import ehealth.data_objects.LoginRequest;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.RegisterRequest;
import ehealth.data_objects.RegisteredUserData;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    String getStrainByEffect(String effectName);

    String getStrainById(String strainId);

    String getAllEffects(String strainName);

    String getAllStrains(String strainName);

    RegisteredUserData authenticate(LoginRequest loginRequest);

    RegisteredUserData register(RegisterRequest registerRequest);
}
