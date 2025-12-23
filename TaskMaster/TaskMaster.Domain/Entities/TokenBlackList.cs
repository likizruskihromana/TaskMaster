using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Domain.Entities
{
    public class TokenBlackList
    {
        public int ID { get; set; }
        public required string TokenId { get; set; }
        public DateTime BlackListedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
