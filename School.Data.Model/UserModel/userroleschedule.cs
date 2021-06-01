namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("userroleschedule")]
    public partial class userroleschedule
    {
        public int UserRoleScheduleId { get; set; }

        public int UserRoleAssocId { get; set; }

        [Required]
        [StringLength(500)]
        public string ScheduleTopic { get; set; }

        [Required]
        [StringLength(1000)]
        public string TopicDesc { get; set; }

        public int BoardGradeSubAssocId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ScheduledDateTime { get; set; }

        [Required]
        [StringLength(10)]
        public string ScheduledDuration { get; set; }

        public int MinCandidateSize { get; set; }

        public int? MaxCandidateSize { get; set; }

        [StringLength(1000)]
        public string TopicRefMaterialBasePath { get; set; }

        [StringLength(8000)]
        public string TopicRefMaterial { get; set; }

        public int ScheduleCost { get; set; }

        //public virtual boardgradesubassoc boardgradesubassoc { get; set; }

        //public virtual userroleassoc userroleassoc { get; set; }
    }
}
