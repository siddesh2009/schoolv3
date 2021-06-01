namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("statesmaster")]
    public partial class statesmaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public statesmaster()
        {
            //citiesmasters = new HashSet<citiesmaster>();
        }

        [Key]
        [StringLength(50)]
        public string StateCode { get; set; }

        [StringLength(100)]
        public string StateName { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<citiesmaster> citiesmasters { get; set; }

        //public virtual countrymaster countrymaster { get; set; }
    }
}
