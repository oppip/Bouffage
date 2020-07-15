using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class VoteComment
    {
        [Key]
        public int VoteCommentId { get; set; }

        public int UserVotedThisComment { set; get; }    //Which user voted

        public User User { get; set; }

        public int CommentGotVoted { set; get; }     //On what comment was the vote

        public Comment Comment { get; set; }

        public char UpOrDown { get; set; }     //Did the user upvote or downvote
    }
}
