using System.ComponentModel.DataAnnotations;

namespace CodersCupAward.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
