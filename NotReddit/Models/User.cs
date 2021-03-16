using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class User : IdentityUser
    {
        public int BadgeId { get; set; }
        public string Name { get; set; }
        public byte[] Imagine { get; set; }
        public Badge UserBadge { get; set; }     

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
