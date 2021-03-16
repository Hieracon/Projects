using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Models
{
    public class Subsection
    {
        public int SubsectionId { get; set; }
        public string SubsectionName { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
