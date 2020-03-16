using System.ComponentModel.DataAnnotations;

namespace ResearchApp.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        public string IPAddress { get; set; }
    }
}
