namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("boardmaster")]
    public partial class boardmaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public boardmaster()
        {
            //boardgradesubassocs = new HashSet<boardgradesubassoc>();
        }

        [Key]
        public int BoardId { get; set; }

        [StringLength(100)]
        public string BoardName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<boardgradesubassoc> boardgradesubassocs { get; set; }
    }
}
