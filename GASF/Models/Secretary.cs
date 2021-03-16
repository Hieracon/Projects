using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class Secretary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SecretaryId { get; set; }
        public string CNP { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
    }
}
