namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("grademaster")]
    public partial class grademaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public grademaster()
        {
            //boardgradesubassocs = new HashSet<boardgradesubassoc>();
        }

        [Key]
        public int GradeId { get; set; }

        [StringLength(100)]
        public string GradeName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<boardgradesubassoc> boardgradesubassocs { get; set; }
    }
}
