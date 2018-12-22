package ehealth.identity;

import org.springframework.stereotype.Service;

import java.util.HashMap;
import java.util.Map;

@Service
public class Identity {
    public Map<String,String> users;

    public Identity(){
        users = new HashMap<>();
        users.put("root","12345678");
    }
}
