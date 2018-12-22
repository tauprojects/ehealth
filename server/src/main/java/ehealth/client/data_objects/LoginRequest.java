package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class LoginRequest {
    @JsonProperty("username")
    String username;
    @JsonProperty("password")
    String password;
}
