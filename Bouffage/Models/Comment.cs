using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bouffage.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [StringLength(500)]
        public string Critique { get; set; }    //The comment itself

        public DateTime? CommentPosted { get; set; }     //When the comment was posted

        public int Useful { get; set; }     //~upvotes

        public int Useless { get; set; }    //~downvotes        

        public enum TypeOfC     //What type of comment is posted
        {
            Question,
            Comment,
            Tip,
            Review
        }

        [Required]
        public virtual int TypeId
        {
            get
            {
                return (int)this.TypeOfComments;
            }
            set
            {
                TypeOfComments = (TypeOfC)value;
            }
        }
        [EnumDataType(typeof(TypeOfC))]
        public TypeOfC TypeOfComments { get; set; }


        public int? UserCommentedId { set; get; }   //which user posted the comment

        public User User { get; set; }

        public int? CommentOnRecipeId { set; get; }     //On which recipe was the comment posted

        public Recipe Recipe { get; set; }

        public int? ReplyCommentId { set; get; }    //If the comment is a reply

        public Comment Reply { get; set; }

        public ICollection<Comment> Replies { get; set; }

        public ICollection<VoteComment> UsersThatVotedComment { get; set; }

    }
}
