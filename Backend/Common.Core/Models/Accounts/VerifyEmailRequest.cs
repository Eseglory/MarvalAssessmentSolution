using System.ComponentModel.DataAnnotations;

namespace Common.Core.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}