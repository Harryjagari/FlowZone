namespace FlowZone.shared.Dtos
{
    public class ResetPasswordWithOTPDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
    }
}
