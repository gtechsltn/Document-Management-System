using System.ComponentModel.DataAnnotations;

namespace DMS.Models
{
    public class UserInfo
    {
        public string Username { get; set; }

        public string Role { get; set; }

        public string BrCode { get; set; }

        public string Division { get; set; }

        public string Department { get; set; }
    }


    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class PassChangeModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfPassword { get; set; }
    }

}
