package ehealth.service.api;

import ehealth.data_objects.*;

import java.util.List;


public interface StrainApiService {

    BaseResponse getStrainByName(String strainName);

    RegisteredUserData authenticate(LoginRequest loginRequest);

    RegisteredUserData register(RegisterRequest registerRequest);

    RecommendedStrainList getRecommendedStrain(String userId);

    void saveUsageHistoryForUser(UsageHistory usageHistory);

    List<UsageHistoryResponse> getUsageHistoryForUser(String userId);
}
