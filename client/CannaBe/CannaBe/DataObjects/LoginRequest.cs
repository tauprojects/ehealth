using System.Collections.Generic;
using System.Net.Http;

namespace CannaBe
{
    class LoginRequest
    {
        string Username { get; set; }
        string Password { get; set; }

        public LoginRequest(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public static implicit operator FormUrlEncodedContent(LoginRequest req)
        {
            var dict = new Dictionary<string, string>
            {
                { req.Username, req.Password }
            };

            return new FormUrlEncodedContent(dict);
        }
    }
}
