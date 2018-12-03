package ehealth.api;

import ehealth.data_objects.BaseResponse;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;
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
    public BaseResponse getStrainEffects(@PathVariable("strain-name") String strainName) {
        return mainServiceImpl.getStrainByName(strainName);
    }

    /**
     * Basic API
     *
     * @return String
     */
    @RequestMapping(value = "/ehealth/strain/{strain-name}", method = RequestMethod.POST)
    public BaseResponse storeUser(@PathVariable("user") String strainName) {
        return mainServiceImpl.getStrainByName(strainName);
    }



}

