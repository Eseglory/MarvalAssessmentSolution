using System.ComponentModel.DataAnnotations;

namespace Common.Core.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}