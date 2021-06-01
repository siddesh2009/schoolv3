namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("userdetail")]
    public partial class userdetail
    {
        public int UserDetailId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public long? MobileContact { get; set; }

        public int SchoolUserId { get; set; }

        [StringLength(5000)]
        public string ProfessionsBkg { get; set; }

        [StringLength(5000)]
        public string PassionateTopic { get; set; }

        [StringLength(3000)]
        public string ProfessionalCertificates { get; set; }

        public string CountryCode { get; set; }

        public string StateCode { get; set; }

        public string CityCode { get; set; }

        public virtual schooluser schooluser { get; set; }
    }
}
