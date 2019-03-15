package ehealth.service.api;

import ehealth.client.data_objects.StrainObject;
import ehealth.client.data_objects.SuggestedStrains;
import ehealth.data_objects.*;

import java.io.IOException;
import java.util.List;
import java.util.Map;


public interface StrainApiService {

    RegisteredUserData authenticate(LoginRequest loginRequest);

    RegisteredUserData register(RegisterRequest registerRequest);

    RegisteredUserData edit(String userId, RegisterRequest registerRequest);

    StrainObject getStrainByName(String strainName);

    SuggestedStrains getRecommendedStrain(String userId);

    void saveUsageHistoryForUser(UsageHistory usageHistory);

    List<UsageHistoryResponse> getUsageHistoryForUser(String userId);

    List<StrainObject> getAllStrains() throws IOException;

    Map<String,Integer> GetListOfStrains();

    BaseResponse exportToEmail(String userId, String to, String content) throws IOException;
}
