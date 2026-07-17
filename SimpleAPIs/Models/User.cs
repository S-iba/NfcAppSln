using System.ComponentModel.DataAnnotations;

namespace SimpleAPIs.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        //Username of the user
        [Required]
        public string Username { get; set; }

        //UUID from RFID tag
        [Required]
        public string UUID { get; set; }
    }
}
