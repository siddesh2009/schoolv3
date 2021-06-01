namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("userroleassoc")]
    public partial class userroleassoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public userroleassoc()
        {
            //boardgradesubassocs = new HashSet<boardgradesubassoc>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserRoleAssocId { get; set; }

        public int SchoolUserId { get; set; }

        public int RoleId { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<boardgradesubassoc> boardgradesubassocs { get; set; }

        //public ICollection<rolemaster> rolemaster { get; set; }

        public virtual schooluser schooluser { get; set; }
    }
}
