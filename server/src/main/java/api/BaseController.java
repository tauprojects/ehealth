package api;

import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@Controller
@RestController
public class BaseController {

	/**
	 * doDownload API
	 * @return String
	 */
	@RequestMapping(value = "/ehealth", method = RequestMethod.GET)
	public String doDownload()  {
		return "eHealth echo server";
	}


}

