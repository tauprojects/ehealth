package ehealth.service.api;

import ehealth.client.data_objects.StrainObject;
import ehealth.data_objects.*;

import java.io.IOException;
import java.util.List;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    RegisteredUserData authenticate(LoginRequest loginRequest);

    RegisteredUserData register(RegisterRequest registerRequest);

    List<StrainObject> getRecommendedStrain(String userId);

    void saveUsageHistoryForUser(UsageHistory usageHistory);

    List<UsageHistoryResponse> getUsageHistoryForUser(String userId);

    List<StrainObject> getAllStrains() throws IOException;
}
