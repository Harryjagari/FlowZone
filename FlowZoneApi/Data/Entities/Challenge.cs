﻿using System.ComponentModel.DataAnnotations;

namespace FlowZoneApi.Data.Entities
{

	public class Challenge
	{
		[Key]
		public Guid ChallengeId { get; set; }

		[Required, MaxLength(30)]
		public string Name { get; set; }

		[Required, MaxLength(100)]
		public string Description { get; set; }

		[Required]
		public int Points { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }

        // Navigation property
        public List<UserChallenge> UserChallenges { get; set; }

	}
}
