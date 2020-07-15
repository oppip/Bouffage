using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Bouffage.Models
{
    public class Following
    {
        [Key]
        public int FollowingId { get; set; }

        [ForeignKey("User")]
        public int UserFollowingId { set; get; }    //user1 is following user2

        public User UserFollowing { get; set; }

        [ForeignKey("User")]
        public int UserFolloweeId { set; get; }     //user2 is followed by user1

        public User UserFollowee { get; set; }

        public DateTime DateFollowed { get; set; }  

    }
}
