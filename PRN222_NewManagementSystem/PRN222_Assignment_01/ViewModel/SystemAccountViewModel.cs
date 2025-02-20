using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PRN222_Assignment_01.ViewModel
{
    public class AccountLogin
    {
        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
