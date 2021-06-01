using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModel
{
    public class UserDetailViewModel
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public int MobileContact { get; set; }
        public string ProfessionsBkg { get; set; }
        public string PassionateTopic { get; set; }
        public string ProfessionalCertificates { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public int UserRoleId { get; set; }
        public List<BoardGradeSubUserAssocViewModel> boardGradeSubUserList { get; set; }
    }

    public class BoardGradeSubUserAssocViewModel
    {
        public int BoardId { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
    }
}
