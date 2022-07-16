using Common.Core.Models.Accounts;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        Task Register(RegisterRequest model, string origin);
        Task VerifyAccount(VerifyEmailRequest model);
        void ResendOtp(ResendOtpRequest model);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(ValidateResetTokenRequest model);
        void ResetPassword(ResetPasswordRequest model);
        AccountResponse GetById(int id);
        AccountResponse Update(int id, UpdateRequest model);
        Task<int> UpdateProfilePicture(int id, string imageName);
    }
}
