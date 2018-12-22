package ehealth.service.api;

import ehealth.data_objects.LoginRequest;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.RegisterRequest;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    String getStrainByEffect(String effectName);

    String getStrainById(String strainId);

    String getAllEffects(String strainName);

    String getAllStrains(String strainName);

    BaseResponse authenticate(LoginRequest loginRequest);

    BaseResponse register(RegisterRequest registerRequest);
}
