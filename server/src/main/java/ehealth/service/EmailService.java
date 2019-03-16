package ehealth.service;

import com.hubspot.jinjava.Jinjava;
import com.sendgrid.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

@Service
public class EmailService {
    public static final String contentTypeHtml = "text/html";
    private Logger logger = LoggerFactory.getLogger(EmailService.class);
    public static final String emailTemplate = "<!DOCTYPE html>\n" +
            "<html>\n" +
            "<head>\n" +
            "<title>Medicanna Email Service</title>\n" +
            "<style>\n" +
            "body {\n" +
            "  background-color: white;\n" +
            "  text-align: left;\n" +
            "  color: green;\n" +
            "  font-family: Arial, Helvetica, sans-serif;\n" +
            "}\n" +
            "</style>\n" +
            "</head>\n" +
            "<body>\n" +
            "\n" +
            "<img src=\"https://i.ibb.co/k0772rJ/Medicanna-Logo.jpg\" alt=\"Avatar\" style=\"width:200px\">\n" +
            "<h3>Hello {{to}}</h3>\n" +
            "<h3> This is a email from Medicanna app</h3>\n" +
            "<p>Client message: {{userContent}}.</p>\n" +
            "<p>----------------------------------------------------------------------</p>\n" +
            "<p>This email contains medical usage history of {{username}}.</p>\n" +
            "<br>\n" +
            "{{usageData}}\n" +
            "<p>Regards,</p>\n" +
            "<p>{{username}}</p>\n" +
            "</body>\n" +
            "</html>\n";
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

    public int sendEmail(String username, String userEmailAddress, String toAddress, String subject,String usageHistory, String userContent) throws IOException {
        Email from = new Email("usage-service@medicannaApp.com");
        Email to = new Email(toAddress);
        Email replyTo = new Email(userEmailAddress);
        String emailContent = renderContent(username,toAddress,usageHistory, userContent);
        Content content = new Content(contentTypeHtml, emailContent);
        Mail mail = new Mail(from, subject, to, content);
        mail.setReplyTo(replyTo);
        return sendEmail(mail);
    }

    private String renderContent(String username, String toAddress, String usageHistory, String userContent) {
        Jinjava jinjava = new Jinjava();
        Map<String, Object> context = new HashMap<>();
        context.put("to",toAddress);
        context.put("userContent", userContent);
        context.put("usageData", usageHistory);
        context.put("username", username);
        return  jinjava.render(emailTemplate, context);
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
