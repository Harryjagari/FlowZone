using System;

namespace FlowZone.shared.Dtos
{
    public class ChallengeDto
    {
        public Guid ChallengeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
