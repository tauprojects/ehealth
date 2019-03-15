using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class UsageUpdateRequest : Request
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("strain_name")]
        public string StrainName { get; set; }

        [JsonProperty("strain_id")]
        public int StrainId { get; set; }

        [JsonProperty("start_time")]
        public long unixStartTime;

        [JsonProperty("end_time")]
        public long unixEndTime;

        [JsonProperty("medical_rank")]
        public int MedicalRank { get; set; }

        [JsonProperty("positive_rank")]
        public int PositiveRank { get; set; }

        [JsonProperty("overall_rank")]
        public int OverallRank { get; set; }

        [JsonProperty("heartbeat_high")]
        public int HeartbeatHigh { get; set; }

        [JsonProperty("heartbeat_low")]
        public int HeartbeatLow { get; set; }

        [JsonProperty("heartbeat_avg")]
        public int HeartbeatAvg { get; set; }

        [JsonProperty("is_blacklist")]
        public int Is_Blacklist { get; set; }

        [JsonProperty("questions_answers_dictionary")]
        public string QuestionsJson;

        [JsonConstructor]
        public UsageUpdateRequest(string strainname, int strainId, string userid, long startTime, long endTime,
            int medicalrank, int positiverank, int overallrank, int heartbeathigh, int heartbeatlow, int heartbeatavg, int is_blacklist,
            Dictionary<string, string> questionDictionary)
        {
            StrainName = strainname;
            StrainId = strainId;
            unixStartTime = startTime;
            unixEndTime = endTime;
            UserID = userid;
            MedicalRank = medicalrank;
            PositiveRank = positiverank;
            OverallRank = overallrank;
            HeartbeatHigh = heartbeathigh;
            HeartbeatLow = heartbeatlow;
            HeartbeatAvg = heartbeatavg;
            Is_Blacklist = is_blacklist;
            QuestionsJson = JsonConvert.SerializeObject(questionDictionary); 
        }
    }
}
