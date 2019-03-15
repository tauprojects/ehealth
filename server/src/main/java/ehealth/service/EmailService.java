package ehealth.service;

import com.sendgrid.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.List;

@Service
public class EmailService {
    public static final String contentTypeText = "text/plain";
    public static final String medicannaFormat = "@medicanna.com";
    private Logger logger = LoggerFactory.getLogger(EmailService.class);

    public SendGrid sg;

    public EmailService() {
        String sendgridApikey = System.getenv("SENDGRID_API_KEY");
        if (sendgridApikey != null) {
            logger.info("Sendgrid APIKEY loaded successfully");
        } else {
            logger.info("Sendgrid APIKEY is missing - Email service may not work");
        }
        this.sg = new SendGrid(sendgridApikey);
    }

    public int sendEmail(String fromUserName, String toAddress, String subject, String contentText) throws IOException {
        Email from = new Email(fromUserName + medicannaFormat);
        Email to = new Email(toAddress);
        Content content = new Content(contentTypeText, contentText);
        Mail mail = new Mail(from, subject, to, content);
        return sendEmail(mail);
    }


    public int sendEmail(Mail mail) throws IOException {
        Response response = null;
        Request request = new Request();
        try {
            request.setMethod(Method.POST);
            request.setEndpoint("mail/send");
            request.setBody(mail.build());
            response = sg.api(request);
            logger.info("Email response status code: " + response.getStatusCode());
            logger.info(response.getBody());
            logger.info(response.getHeaders().toString());
        } catch (IOException ex) {
            throw ex;
        }
        return response.getStatusCode();
    }

}
