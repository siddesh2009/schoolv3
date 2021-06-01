namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("refreshtoken")]
    public partial class refreshtoken
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public refreshtoken()
        {
            schoolusers = new HashSet<schooluser>();
        }

        public int RefreshTokenId { get; set; }

        public string Token { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Expires { get; set; }

        public short IsExpired { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedDateTime { get; set; }

        [Required]
        [StringLength(1000)]
        public string CreatedByIp { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RevokedDateTime { get; set; }

        [StringLength(1000)]
        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public short IsActive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schooluser> schoolusers { get; set; }
    }
}
