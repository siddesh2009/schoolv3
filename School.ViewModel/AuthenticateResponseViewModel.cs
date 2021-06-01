using Newtonsoft.Json;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModel
{
    public class AuthenticateResponseViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponseViewModel(schooluser user, string jwtToken, string refreshToken)
        {
            Id = user.SchoolUserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            EmailId = user.EmailId;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
