using System.ComponentModel.DataAnnotations;

namespace Common.Core.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
    public class ResendOtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}