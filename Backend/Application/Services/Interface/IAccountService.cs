using Common.Core.Models.Accounts;
using Common.Entities;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
    }
}
