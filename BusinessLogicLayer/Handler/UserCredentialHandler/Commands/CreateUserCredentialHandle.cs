using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using DatabaseAccessLayer.Entities;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.Commands
{
    public class CreateUserCredentialHandle
        : IRequestHandler<CreateUserCredentialHandleRequest, CreateUserCredentialHandleResponse>
    {
        private const string LocalProvider = "Local";

        private readonly IUserRepository _userRepo;
        private readonly IUserCredentialRepository _credRepo;
        private readonly IPasswordHasher<UserCredential> _hasher;

        public CreateUserCredentialHandle(
            IUserRepository userRepo,
            IUserCredentialRepository credRepo,
            IPasswordHasher<UserCredential> hasher)
        {
            _userRepo = userRepo;
            _credRepo = credRepo;
            _hasher = hasher;
        }

        public async Task<CreateUserCredentialHandleResponse> Handle(
            CreateUserCredentialHandleRequest request,
            CancellationToken cancellationToken)
        {
            // ---- Validasyonlar ----
            if (request is null)
                return Fail("İstek boş olamaz.");

            var user = _userRepo.GetById(request.UserId);
            if (user is null)
                return Fail("Kullanıcı bulunamadı.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return Fail("E-posta zorunludur.");

            try { _ = new MailAddress(request.Email); }
            catch { return Fail("E-posta formatı geçersiz."); }

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                return Fail("Parola en az 6 karakter olmalıdır.");

            // E-posta kullanımda mı? (provider = Local)
            var existingByEmail = await _credRepo.GetByEmailAsync(request.Email);
            if (existingByEmail is not null)
                return Fail("Bu e-posta zaten kullanılmakta.");

            // Bu kullanıcı için zaten credential var mı? (1-1 ilişki)
            var existingByUser = await _credRepo.GetByUserIdAsync(request.UserId);
            if (existingByUser is not null)
                return Fail("Bu kullanıcı için zaten kimlik oluşturulmuş.");

            // ---- Oluşturma ----
            var cred = new UserCredential
            {
                UserId = request.UserId,
                Email = request.Email.Trim(),
                Provider = LocalProvider,
                LockoutEnabled = request.LockoutEnabled,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };

            cred.PasswordHash = _hasher.HashPassword(cred, request.Password);

            await _credRepo.AddAsync(cred); // Not: Senin repo AddAsync() içinde SaveChanges yapıyor.

            return new CreateUserCredentialHandleResponse
            {
                Error = false,
                Message = "Kullanıcı kimliği başarıyla oluşturuldu.",
                CredentialId = cred.Id,
                UserId = cred.UserId,
                Email = cred.Email,
                EmailConfirmed = cred.EmailConfirmed,
                CreatedDate = cred.CreatedDate
            };
        }

        private static CreateUserCredentialHandleResponse Fail(string message) => new()
        {
            Error = true,
            Message = message
        };
    }
}
