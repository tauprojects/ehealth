package ehealth.service.api;

import ehealth.client.data_objects.LoginRequest;
import ehealth.data_objects.BaseResponse;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    String getStrainByEffect(String effectName);

    String getStrainById(String strainId);

    String getAllEffects(String strainName);

    String getAllStrains(String strainName);

    BaseResponse authenticate(LoginRequest loginRequest);
}
