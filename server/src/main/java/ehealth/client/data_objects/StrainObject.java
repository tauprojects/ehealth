package ehealth.client.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import lombok.*;
import org.codehaus.jackson.map.util.JSONPObject;

import java.util.List;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class StrainObject {
    String name;
    String id;
    String race;
    Long positive;
    Long negative;
    Long medical;
}

