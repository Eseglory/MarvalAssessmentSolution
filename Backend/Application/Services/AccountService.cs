using AutoMapper;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Common.Core.Models.Accounts;
using Infrastructure.Persistence;
using Application.Services.Interface;
using Common.Core.Helpers;
using Common.Entities;
using Common.Enums;

namespace Application.Service
{

    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public AccountService(
            DataContext context,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            if (account == null)
            {
                throw new AppException("This user does not exist");
            }
            else if (!account.IsVerified)
            {
                throw new AppException("This account has not been verified, an otp was sent to your email when you registered.");
            }

            if (!BC.Verify(model.Password, account.PasswordHash))
                throw new AppException("Email or password is incorrect");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(account);
            var refreshToken = generateRefreshToken(ipAddress);
            account.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(account);

            // save changes to db
            _context.Update(account);
            var result = await _context.SaveChangesAsync();

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken.Token;
            return response;
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);

            removeOldRefreshTokens(account);

            _context.Update(account);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(account);

            var response = _mapper.Map<AuthenticateResponse>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken.Token;
            return response;
        }

        public void RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(account);
            _context.SaveChanges();
        }

        public async Task Register(RegisterRequest model, string origin)
        {
            // validate
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {
                throw new AppException("Email already registered, Please check your inbox");
            }
            if (_context.Accounts.Any(x => x.PhoneNumber == model.PhoneNumber))
            {
                throw new AppException("Phone Number already registered");
            }

            // map model to new account object
            var account = _mapper.Map<Account>(model);

            account.Role = Role.User;
            account.DateCreated = DateTime.UtcNow;
            account.VerificationToken = CodeGenerator.RandomNumber(6);
            account.TokenDate = DateTime.UtcNow;

            // hash password
            account.PasswordHash = BC.HashPassword(model.Password);

            // save account
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // send email
           // sendVerificationEmail(account, origin);
        }

        public async Task VerifyAccount(VerifyEmailRequest model)
        {
            try
            {
                var account = _context.Accounts.SingleOrDefault(x => x.VerificationToken == model.Otp && x.Email == model.Email);

                if (account == null)
                {
                    throw new AppException("Verification failed");
                }
                else if (account.IsVerified)
                {
                    throw new AppException("This account has already been verified.");
                }

                if (DateTime.Now.Subtract(account.TokenDate) > TimeSpan.FromMinutes(30))
                {
                    throw new AppException("Your OTP has expired, please resend OTP.");
                }

                account.Verified = DateTime.UtcNow;
                account.VerificationToken = null;

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
              
            }
            catch (Exception e)
            {
                throw new AppException($"Network Error!");
            }
        }

        public void ResendOtp(ResendOtpRequest model)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Email == model.Email);
            if (account == null)
            {
                throw new AppException($"No user with the email address {model.Email} exist in the system , Please do sign-up.");
            }

            account.Role = Role.User;
            account.DateCreated = DateTime.Now;
            account.TokenDate = DateTime.UtcNow;
            account.VerificationToken = CodeGenerator.RandomNumber(6);

            _context.Accounts.Update(account);
            _context.SaveChanges();

           // sendVerificationEmail(account, "");
        }

        public void ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            // create reset token that expires after 1 day
            account.ResetToken = CodeGenerator.RandomNumber(6);
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            _context.Accounts.Update(account);
            _context.SaveChanges();

            // send email
           // sendPasswordResetEmail(account, origin);
        }

        public void ValidateResetToken(ValidateResetTokenRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null)
                throw new AppException("Invalid token");
        }

        public void ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var account = _context.Accounts.SingleOrDefault(x =>
                    x.ResetToken == model.Token &&
                    x.ResetTokenExpires > DateTime.UtcNow);

                if (account == null)
                    throw new AppException("Invalid token");

                if(model.Password != model.ConfirmPassword)
                    throw new AppException("ConfirmPassword is wrong!");

                // update password and remove reset token
                account.PasswordHash = BC.HashPassword(model.Password);
                account.PasswordReset = DateTime.UtcNow;
                account.ResetToken = null;
                account.ResetTokenExpires = null;

                _context.Accounts.Update(account);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw new AppException("Invalid entry, please check your records.");
            }
        }

        public AccountResponse GetById(int id)
        {
            var account = getAccount(id);
            return _mapper.Map<AccountResponse>(account);
        }

        public AccountResponse Update(int id, UpdateRequest model)
        {
            var account = getAccount(id);

            // validate
            if (account.Email != model.Email && _context.Accounts.Any(x => x.Email == model.Email))
                throw new AppException($"Email '{model.Email}' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                account.PasswordHash = BC.HashPassword(model.Password);

            // copy model to account and save
            _mapper.Map(model, account);
            account.DateUpdated = DateTime.UtcNow;
            _context.Accounts.Update(account);
            _context.SaveChanges();

            return _mapper.Map<AccountResponse>(account);
        }

        private Account getAccount(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }

        private (RefreshToken, Account) getRefreshToken(string token)
        {
            var account = _context.Accounts.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        private string generateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = CodeGenerator.randomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private void removeOldRefreshTokens(Account account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }


        public async Task<int> UpdateProfilePicture(int id, string imageName)
        {
            try
            {
                var account = getAccount(id);

                if (account == null)
                    throw new AppException($"This user deos not exist.");
                account.DateUpdated = DateTime.UtcNow;
                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                throw new AppException($"This user does not exist." + ex.Message);
            }
        }

    }
}
