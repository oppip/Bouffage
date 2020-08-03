using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.ViewModels
{
    [Table("User")]
    public class SignUpViewModel
    {
        [Required]
        [DisplayName("Е-пошта")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Корисничко име")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Лозинка")]
        [DataType("Password")]
        public string Password { get; set; }

        [Required]
        [DisplayName("Потврдете ја лозинката")]
        [DataType("Password")]
        public string ConfirmedPassword { get; set; }

        public DateTime DateCreated { get; set; }

        public string Role { get; set; }

        public int Karma { get; set; }

        public int Following { get; set; }

        public int Followed { get; set; }

        public bool VerifiedEmail { get; set; }

        public SignUpViewModel()
        {
            DateCreated = DateTime.UtcNow;
            Role = "User";
            Karma = 0;
            Followed = 0;
            Following = 0;
        }
    }
}
