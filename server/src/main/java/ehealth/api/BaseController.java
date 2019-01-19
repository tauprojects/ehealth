package ehealth.api;

import ehealth.data_objects.LoginRequest;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.RegisterRequest;
import ehealth.data_objects.RegisteredUserData;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import ehealth.service.StrainApiServiceImpl;

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
        logger.info("GET strain information: " + strainName);
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
     * Register API
     *
     * @return String
     */
    @RequestMapping(value = "/strains/recommended", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RegisteredUserData getRecommendedStrains(@RequestParam RegisterRequest registerRequest) {
        logger.info("POST register request: " + registerRequest.toString());
        return mainServiceImpl.register(registerRequest);
    }
}

