namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("countrymaster")]
    public partial class countrymaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public countrymaster()
        {
            //statesmasters = new HashSet<statesmaster>();
        }

        [Key]
        [StringLength(50)]
        public string CountryCode { get; set; }

        [StringLength(100)]
        public string CountryName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<statesmaster> statesmasters { get; set; }
    }
}
