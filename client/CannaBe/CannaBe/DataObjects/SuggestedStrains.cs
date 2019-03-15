
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CannaBe
{
    class SuggestedStrains
    {
        [JsonProperty("status")]
        public int status;

        [JsonProperty("suggestedStrains")]
        public List<Strain> suggestedStrains;

        [JsonConstructor]
        public SuggestedStrains(int status, List<Strain> suggestedStrains)
        {
            this.status = status;
            this.suggestedStrains = suggestedStrains;
        }
    }
}
