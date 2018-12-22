package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class Effect {
    @JsonProperty("effect")
    Integer effect;
    @JsonProperty("type")
    String type;
}
