package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;


@EqualsAndHashCode
@AllArgsConstructor
@Getter
@Setter
@NoArgsConstructor
@ToString
public class EmailRequest {
    @JsonProperty("to")
    String to;
    @JsonProperty("content")
    String content;
}
