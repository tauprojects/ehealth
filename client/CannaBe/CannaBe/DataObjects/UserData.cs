using System.Collections.Generic;

namespace CannaBe
{
    class UserData
    {
        public LoginResponse Data { get; }
        public List<UsageData> UsageSessions;

        public UserData(LoginResponse data)
        {
            Data = data;
            UsageSessions = new List<UsageData>();
        }
    }
}
