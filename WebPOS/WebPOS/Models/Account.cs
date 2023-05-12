using System.ComponentModel.DataAnnotations;

namespace WebPOS.Models
{
    public class Account
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Roles { get; set; }
    }
}