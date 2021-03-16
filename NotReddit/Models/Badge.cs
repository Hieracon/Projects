using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class Badge
    {
        public int BadgeId { get; set; }
        public string BadgeName { get; set; }
        public int RequiredNrLikes { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}
