using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bouffage.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required] //should be unique
        [StringLength(35)]
        /*[Display(Name = "Е-пошта")]*/
        public string Email { get; set; }

        [StringLength(60)]
        /*[Display(Name = "Лозинка")]*/
        public string Password { get; set; }

        [StringLength(40)]
        /*[Display(Name = "Корисничко име")]*/
        public string Username { get; set; }

        public DateTime? DateCreated { get; set; }

        [StringLength(20)]
        public string Role { get; set; }        //	What privileges does the account have

        public int Karma { get; set; }

        public int Following { get; set; }      //Which users does this account follow

        public int Followers { get; set; }      //Which users follow this account	

        public string Picture { get; set; } //profile picture

        public bool VerifiedEmail { get; set; }

        public ICollection<Following> IFollow { get; set; }

        public ICollection<Following> FollowMe { get; set; }

        public ICollection<Comment> MyComments { get; set; }

        public ICollection<Recipe> MyRecipes { get; set; }

        public ICollection<VoteRecipe> VotedRecipes { get; set; }

        public ICollection<VoteComment> VotedComments { get; set; }
    }
}
