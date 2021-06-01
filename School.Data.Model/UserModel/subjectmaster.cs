namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("subjectmaster")]
    public partial class subjectmaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public subjectmaster()
        {
            //boardgradesubassocs = new HashSet<boardgradesubassoc>();
        }

        [Key]
        public int SubjectId { get; set; }

        [StringLength(100)]
        public string SubjectName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<boardgradesubassoc> boardgradesubassocs { get; set; }
    }
}
