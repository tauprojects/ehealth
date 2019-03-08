package ehealth.client.data_objects;

import lombok.*;



@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class StrainObject {
    String name;
    Integer id;
    String race;
    String summary;
    Long positive = new Long(0);
    Long negative = new Long(0);
    Long medical = new Long(0);
}
