package ehealth.service.api;

import ehealth.data_objects.BaseResponse;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    String getStrainByEffect(String effectName);

    String getStrainById(String strainId);

    String getAllEffects(String strainName);

    String getAllStrains(String strainName);

}
