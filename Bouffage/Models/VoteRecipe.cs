using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class VoteRecipe
    {
        [Key]
        public int VoteRecipeId { get; set; }

        public int UserVotedThisRecipe { set; get; }    //Which user voted

        public User User { get; set; }

        public int RecipeGotVoted { set; get; }     //On what recipe was the vote

        public Recipe Recipe { get; set; }

        public char UpOrDown { get; set; }     //Did the user upvote or downvote

    }
}
