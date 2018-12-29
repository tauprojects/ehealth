package ehealth.api;

import ehealth.data_objects.LoginRequest;
import ehealth.data_objects.BaseResponse;
import ehealth.data_objects.RegisterRequest;
import ehealth.data_objects.RegisteredUserData;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import ehealth.service.StrainApiServiceImpl;

@Controller
@RestController
public class BaseController {

    private StrainApiServiceImpl mainServiceImpl;

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
        return "echo server:" + strainName;
    }

    /**
     * Login API
     *
     * @return String
     */
    @RequestMapping(value = "/login", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RegisteredUserData login(@RequestBody LoginRequest loginRequest) {
        return mainServiceImpl.authenticate(loginRequest);
    }

        /**
     * Register API
     *
     * @return String
     */
    @RequestMapping(value = "/register", method = RequestMethod.POST, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public RegisteredUserData register(@RequestBody RegisterRequest registerRequest) {
        return mainServiceImpl.register(registerRequest);
    }
}

