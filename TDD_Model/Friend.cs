using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD_Model
{
    public class Friend
    {
        public int Id { get; set; }
        public int FriendGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsDeveloper { get; set; }
        public Address Address { get; set; }
        public List<FriendEmail> Emails { get; set; }
    }
}
