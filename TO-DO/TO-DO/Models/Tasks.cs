using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TO_DO.Models
{
    public partial class Tasks
    {
        [Key]
        public int TaskID { get; set; }
        public string TaskTitle { get; set; } = null!;
        public string TaskDetails { get; set; } = null!;
        public DateTime DateAdded { get; set; }
    }
}
