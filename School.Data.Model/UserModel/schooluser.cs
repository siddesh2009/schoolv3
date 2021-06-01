namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("schooluser")]
    public partial class schooluser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public schooluser()
        {
            userdetails = new HashSet<userdetail>();
            userroleassocs = new HashSet<userroleassoc>();
        }

        public int SchoolUserId { get; set; }

        [Required]
        [StringLength(1000)]
        public string FirstName { get; set; }

        [StringLength(500)]
        public string LastName { get; set; }

        [Required]
        [StringLength(1000)]
        public string EmailId { get; set; }

        [Required]
        [StringLength(5000)]
        public string Password { get; set; }

        public int? RefreshTokenId { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CreatedDateTime { get; set; }

        public int? ModifiedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDateTime { get; set; }

        public virtual refreshtoken refreshtoken { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userdetail> userdetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userroleassoc> userroleassocs { get; set; }
    }
}
