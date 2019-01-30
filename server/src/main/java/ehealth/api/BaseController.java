package ehealth.api;

import ehealth.data_objects.*;
import ehealth.service.StrainApiServiceImpl;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Controller
@RestController
public class BaseController {

    private StrainApiServiceImpl mainServiceImpl;
    private Logger logger = LoggerFactory.getLogger(BaseController.class.getName());

    @Autowired
    public BaseController(StrainApiServiceImpl mainServiceImpl) {
        this.mainServiceImpl = mainServiceImpl;
    }

    /**
     * Basic API
     *
     * @return String
     */
    @RequestMapping(value = "/ehealth/strain/{strain-name}", method = RequestMethod.GET)
    public BaseResponse getStrainInfo(@PathVariable("strain-name") String strainName) {
        return mainServiceImpl.getStrainByName(strainName);
    }

    /**
     * Basic API
     *
     * @return String
     */
    @RequestMapping(value = "/ehealth/effects/{strain-name}", method = RequestMethod.GET)
    public String getStrainEffects(@PathVariable("strain-name") String strainName) {
        logger.info("GET strain effect: " + strainName);

        return "echo server:" + strainName;
    }

    /**
     * Login API
     *
     * @return String
     */
    @RequestMapping(value = "/login", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RegisteredUserData login(@RequestBody LoginRequest loginRequest) {
        logger.info("POST login request: " + loginRequest.toString());
        return mainServiceImpl.authenticate(loginRequest);
    }

    /**
     * Register API
     *
     * @return String
     */
    @RequestMapping(value = "/register", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RegisteredUserData register(@RequestBody RegisterRequest registerRequest) {
        logger.info("POST register request: " + registerRequest.toString());
        return mainServiceImpl.register(registerRequest);
    }

    /**
     * Get recommended API
     *
     * @return List<Strain>
     */
    @RequestMapping(value = "/strains/recommended/{user-id}", method = RequestMethod.GET, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RecommendedStrainList getRecommended(@PathVariable("user-id") String userId) {
        logger.info("GET recommended strains for userId: " + userId);
        // get User Info by ID from database
        RecommendedStrainList resp = mainServiceImpl.getRecommendedStrain(userId);
        return resp;
    }

        /**
     * POST save user usage history API
     *
     * @return BaseResponse
     */
    @RequestMapping(value = "/usage/", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public BaseResponse saveUserUsageHistory(@RequestBody UsageHistory usageHistory) {
        logger.info("POST Usage history for user-id: " + usageHistory.getUserId());
        logger.info("Usage History Request: " + usageHistory.toString());
        mainServiceImpl.saveUsageHistoryForUser(usageHistory);
        return new BaseResponse("Usage successfully stored in database");
    }

    /**
     * GET user usages history API
     *
     * @return List<UsageHistoryResponse>
     */
    @RequestMapping(value = "/usage/{user-id}", method = RequestMethod.GET, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public List<UsageHistoryResponse> saveUserUsageHistory(@PathVariable("user-id") String userId) {
        logger.info("POST Usage history for user-id: " + userId);
        return mainServiceImpl.getUsageHistoryForUser(userId);
    }
}

