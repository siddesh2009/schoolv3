using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.Spatial;

namespace SchoolModel.Model
{
    [Table("boardgradesubassoc")]
    public partial class boardgradesubassoc
    {
        public int BoardGradeSubAssocId { get; set; }

        public int BoardId { get; set; }

        public int GradeId { get; set; }

        public int SubjectId { get; set; }

        public int UserRoleAssocId { get; set; }

        //public virtual boardmaster boardmaster { get; set; }

        //public virtual grademaster grademaster { get; set; }

        //public virtual subjectmaster subjectmaster { get; set; }

        //public virtual userroleassoc userroleassoc { get; set; }
    }
}
