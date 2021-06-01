using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolModel.Model
{
    [Table("errorLogger")]
    public partial class errorLogger
    {
        [Key]
        public int ErrorLoggerId { get; set; }

        [StringLength(9000)]
        public string ErrorLoggerMessage { get; set; }
    }
}
