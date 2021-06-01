using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.ViewModel
{
    public class UserRoleScheduleFileUpload {

        public int UserId { get; set; }
        public ICollection<IFormFile> files { get; set; }
        public string userRoleSchedule { get; set; }
        //public List<UserRoleScheduleViewModel> userRoleSchedule { get; set; }
        
    }
    public class UserRoleScheduleViewModel
    {
        public int UserRoleScheduleId { get; set; }
        public int UserRoleAssocId { get; set; }
        public string ScheduleTopic { get; set; }
        public string TopicDesc { get; set; }
        public int BoardGradeSubAssocId { get; set; }
        public int BoardId { get; set; }
        public string BoardName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string ScheduledDuration { get; set; }
        public int MinCandidateSize { get; set; }
        public int? MaxCandidateSize { get; set; }
        public string TopicRefMaterialBasePath { get; set; }
        public string TopicRefMaterial { get; set; }
        public int ScheduleCost { get; set; }
    }

    public class UserRoleScheduleRequestViewModel 
    {
        public int UserRoleScheduleId { get; set; }
        public int UserRoleAssocId { get; set; }
    }

    public class UserRoleScheduleDocReqViewModel
    {
        public int UserRoleScheduleId { get; set; }
        public string UserRoleScheduleDocName { get; set; }
    }
}
