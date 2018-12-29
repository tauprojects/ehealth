package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import lombok.*;

import java.util.List;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class RegisterRequest {

    @JsonProperty("username")
    String username;

    @JsonProperty("password")
    String password;

    @JsonProperty("dob")
    String DOB;

    @JsonProperty("gender")
    String gender;

    @JsonProperty("country")
    String country;

    @JsonProperty("city")
    String city;

    @JsonProperty("medical")
    List<MedicalEffects> medical;

    @JsonProperty("positive")
    List<PositiveEffects> positive;

    @JsonProperty("negative")
    List<NegativeEffects> negative;

}