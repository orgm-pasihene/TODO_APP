using System.ComponentModel.DataAnnotations;

namespace TO_DO.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
