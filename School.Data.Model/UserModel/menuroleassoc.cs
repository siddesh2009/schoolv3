namespace SchoolModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("menuroleassoc")]
    public partial class menuroleassoc
    {
        [Key]
        [Column("MenuRoleAssoc")]
        public int MenuRoleAssoc1 { get; set; }

        public int? RoleId { get; set; }

        public int? MenuId { get; set; }

        public virtual menumaster menumaster { get; set; }

        public virtual rolemaster rolemaster { get; set; }
    }
}
