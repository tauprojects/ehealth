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
    int medical;

    @JsonProperty("positive")
    int positive;

    @JsonProperty("negative")
    int negative;

    @JsonProperty("created_at")
    Long createdAt;

}
