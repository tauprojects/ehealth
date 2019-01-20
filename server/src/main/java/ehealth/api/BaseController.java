package ehealth.api;

import ehealth.client.data_objects.Strain;
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

import java.util.ArrayList;
import java.util.Arrays;
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
    public List<Strain> getRecommended(@PathVariable("user-id") String userId) {
        logger.info("GET recommended strains for userId: " + userId);
        // get User Info by ID from database
        Strain exampleA = new Strain();
        exampleA.setName("strain-A");
        Strain exampleB = new Strain();
        exampleB.setName("strain-B");
        return Arrays.asList(exampleA,exampleB);
    }
}

