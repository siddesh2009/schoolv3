using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModel
{
    public class UserViewModel
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public long? MobileContact { get; set; }

        public int SchoolUserId { get; set; }

        public string ProfessionsBkg { get; set; }

        public string PassionateTopic { get; set; }

        public string ProfessionalCertificates { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public List<RoleMasterViewModel> userRoleViewModel { get; set; }

        public List<UserBoardGradeSubViewModel> userBoardGradeSubViewModel { get; set; }
    }

    public class UserRoleViewModel { 
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserBoardGradeSubViewModel {
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
