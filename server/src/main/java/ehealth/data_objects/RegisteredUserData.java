package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import lombok.*;

import java.sql.Timestamp;
import java.util.List;
import java.util.UUID;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class RegisteredUserData {
    @JsonProperty("user_id")
    UUID userId;

    @JsonProperty("username")
    String username;

    @JsonProperty("dob")
    String DOB;

    @JsonProperty("gender")
    String gender;

    @JsonProperty("country")
    String country;

    @JsonProperty("city")
    String city;

    @JsonProperty("medical")
    List<String> medical;

    @JsonProperty("positive")
    List<String> positive;

    @JsonProperty("negative")
    List<String> negative;

    @JsonProperty("created_at")
    Long createdAt;

}
