namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("citiesmaster")]
    public partial class citiesmaster
    {
        [Key]
        [StringLength(50)]
        public string CityCode { get; set; }

        [StringLength(100)]
        public string CityName { get; set; }

        [StringLength(50)]
        public string StateCode { get; set; }

        //public virtual statesmaster statesmaster { get; set; }
    }
}
