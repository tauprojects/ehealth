package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;

import java.util.List;


//@NoArgsConstructor
@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class StrainEffects {
    @JsonProperty("positive")
    List<String> positive;
    @JsonProperty("negative")
    List<String> negative;
    @JsonProperty("medical")
    List<String> medical;
}
