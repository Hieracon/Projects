using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class Like
    {
        public int LikeId { get; set; }
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public int NrLikes { get; set; }

        public Post Post { get; set; }
        public Comment Comment { get; set; }
    }
}
