using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.Commands
{
    public sealed class CreateUserCredentialHandle
        : IRequestHandler<CreateUserCredentialHandleRequest, CreateUserCredentialHandleResponse>
    {
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
            string? message = null;

            // --- Girdi kontrolleri ---
            if (request == null)
            {
                message = "İstek boş olamaz.";
            }
            else
            {
                // Kullanıcı var mı?
                var user = _userRepo.GetById(request.UserId);
                if (user is null)
                {
                    message = "Kullanıcı bulunamadı.";
                }

                // E-posta formatı
                if (message == null)
                {
                    if (string.IsNullOrWhiteSpace(request.Email))
                        message = "E-posta zorunludur.";
                    else
                    {
                        try { _ = new MailAddress(request.Email); }
                        catch { message = "E-posta formatı geçersiz."; }
                    }
                }

                // Parola politikası (minimum 6 karakter)
                if (message == null && (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6))
                    message = "Parola en az 6 karakter olmalıdır.";

                // E‑posta kullanımda mı?
                if (message == null)
                {
                    var existingByEmail = await _credRepo.GetByEmailAsync(request.Email);
                    if (existingByEmail is not null)
                        message = "Bu e‑posta zaten kullanılmakta.";
                }

                // Bu user için credential var mı? (1-1 ilişki)
                if (message == null && _credRepo is IHasUserIdLookup hasUserIdLookup)
                {
                    var existingByUser = await hasUserIdLookup.GetByUserIdAsync(request.UserId);
                    if (existingByUser is not null)
                        message = "Bu kullanıcı için zaten kimlik oluşturulmuş.";
                }
            }

            if (message != null)
            {
                return new CreateUserCredentialHandleResponse
                {
                    Error = true,
                    Message = message
                };
            }

            // --- Oluşturma ---
            var cred = new UserCredential
            {
                UserId = request.UserId,
                Email = request.Email.Trim(),
                Provider = "Local",
                LockoutEnabled = request.LockoutEnabled,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };
            cred.PasswordHash = _hasher.HashPassword(cred, request.Password);

            await _credRepo.AddAsync(cred);

            if (_credRepo is IUnitOfWorkLike uow)
                await uow.SaveChangesAsync();

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
    }

    // (Opsiyonel) Repo implementasyonunda varsa UserId ile lookup.
    public interface IHasUserIdLookup
    {
        Task<UserCredential?> GetByUserIdAsync(long userId);
    }

    // (Opsiyonel) SaveChanges mantığını repository üzerinden tetiklemek istersen.
    public interface IUnitOfWorkLike
    {
        Task<int> SaveChangesAsync();
    }
}
