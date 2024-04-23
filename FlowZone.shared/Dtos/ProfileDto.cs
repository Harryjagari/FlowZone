namespace FlowZone.shared.Dtos
{
    public class ProfileDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int EarnedPoints { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
