using System.ComponentModel.DataAnnotations;

namespace TO_DO.Dtos
{
    public class TasksDto
    {
        public int TaskID { get; set; }

        [Required]
        public string TaskTitle { get; set; } = null!;
        [Required]
        public string TaskDetails { get; set; } = null!;
        public DateTime DateAdded { get; set; }

    }
}
