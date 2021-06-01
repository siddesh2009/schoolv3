using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModel
{
    public class RoleMasterViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class CountryMasterViewModel
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

    public class StateMasterViewModel
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryCode { get; set; }
    }

    public class CityMasterViewModel
    {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string StateCode { get; set; }
    }

    public class BoardMasterViewModel
    {
        public int BoardId { get; set; }
        public string BoardName { get; set; }
    }

    public class GradeMasterViewModel
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }
    }

    public class SubjectMasterViewModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
