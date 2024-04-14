using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowZone.shared.Dtos
{
    public class UserChallengeDto
    {
        public Guid UserChallengeId { get; set; }
        public Guid ChallengeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public DateTime EndDate { get; set; }
    }
}