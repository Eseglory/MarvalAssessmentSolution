using System.ComponentModel.DataAnnotations;

namespace Common.Core.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}