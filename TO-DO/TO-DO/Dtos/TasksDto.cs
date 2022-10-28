using System.ComponentModel.DataAnnotations;

namespace TO_DO.Dtos
{
    public class TasksDto
    {
        public int TaskID { get; set; }

        [Required]
        public string TaskTitle { get; set; }
        [Required]
        public string TaskDetails { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
