using CannaBe.Enums;
using System.Collections.Generic;

namespace CannaBe
{
    class UserData
    {
        public LoginResponse Data { get; }

        public UserData(LoginResponse data)
        {
            Data = data;
        }
    }
}
