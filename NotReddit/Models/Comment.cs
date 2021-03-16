using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public int CommentLike { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }

        //public Like Like { get; set; }
    }
}
