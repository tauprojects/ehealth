package ehealth.data_objects;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.ToString;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@ToString
public class UsageHistory {
    @JsonProperty("user_id")
    private String userId;

    @JsonProperty("strain_name")
    private String strainName;

    @JsonProperty("medical_rank")
    private Integer medicalRank;

    @JsonProperty("positive_rank")
    private Integer positiveRank;

    @JsonProperty("overall_rank")
    private Integer overallRank;

    @JsonProperty("heartbeat_high")
    private Integer heartbeatHigh;

    @JsonProperty("heartbeat_low")
    private Integer heartbeatLow;

    @JsonProperty("heartbeat_avg")
    private Integer heartbeatAvg;

}
