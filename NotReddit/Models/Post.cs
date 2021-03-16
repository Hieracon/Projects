using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class Post
    {
        public int PostId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public int SubsectionId { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public string PostLink { get; set; }
        public int PostLike { get; set; }
       

        public User User { get; set; }
        public Subsection Subsection { get; set; }

        //public Like Like { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
