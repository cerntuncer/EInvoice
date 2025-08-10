using System;
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
            var user =  _userRepo.GetById(request.UserId);
            if (user is null)
                throw new InvalidOperationException("User bulunamadı.");

            // 2) E‑posta kullanımda mı?
            var existingByEmail = await _credRepo.GetByEmailAsync(request.Email);
            if (existingByEmail is not null)
                throw new InvalidOperationException("Bu e‑posta zaten kullanılmakta.");

            // 3) Bu user için credential var mı? (1-1 ilişki)
            // Eğer arayüzünde varsa kullan; yoksa DB unique zaten engeller.
            if (_credRepo is IHasUserIdLookup hasUserIdLookup)
            {
                var existingByUser = await hasUserIdLookup.GetByUserIdAsync(request.UserId);
                if (existingByUser is not null)
                    throw new InvalidOperationException("Bu kullanıcı için zaten kimlik oluşturulmuş.");
            }

            // 4) Oluştur
            var cred = new UserCredential
            {
                UserId = request.UserId,
                Email = request.Email.Trim(),
                Provider = "Local",
                LockoutEnabled = request.LockoutEnabled,
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
            };

            cred.PasswordHash = _hasher.HashPassword(cred, request.Password);

            await _credRepo.AddAsync(cred);


            if (_credRepo is IUnitOfWorkLike uow)
                await uow.SaveChangesAsync();

            return new CreateUserCredentialHandleResponse
            {
                CredentialId = cred.Id,
                UserId = cred.UserId,
                Email = cred.Email,
                EmailConfirmed = cred.EmailConfirmed,
                CreatedDate = cred.CreatedDate
            };
        }
    }

    /// <summary>
    /// (Opsiyonel) Repo implementasyonunda varsa UserId ile lookup.
    /// </summary>
    public interface IHasUserIdLookup
    {
        Task<UserCredential?> GetByUserIdAsync(long userId);
    }

    /// <summary>
    /// (Opsiyonel) SaveChanges mantığını repository üzerinden tetiklemek istersen.
    /// Projende UoW varsa bu interface'e gerek yok.
    /// </summary>
    public interface IUnitOfWorkLike
    {
        Task<int> SaveChangesAsync();
    }
}
