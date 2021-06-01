namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("menumaster")]
    public partial class menumaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public menumaster()
        {
            menuroleassocs = new HashSet<menuroleassoc>();
        }

        [Key]
        public int MenuId { get; set; }

        [StringLength(50)]
        public string MenuName { get; set; }

        [StringLength(5000)]
        public string MenuPath { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDateTime { get; set; }

        public int? ModifiedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<menuroleassoc> menuroleassocs { get; set; }
    }
}
