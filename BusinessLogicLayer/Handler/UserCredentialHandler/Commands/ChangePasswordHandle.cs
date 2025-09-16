using System.Threading;
using System.Threading.Tasks;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using DatabaseAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.Commands
{
    public sealed class ChangePasswordHandle : IRequestHandler<ChangePasswordHandleRequest, ChangePasswordHandleResponse>
    {
        private readonly IUserCredentialRepository _credRepo;
        private readonly IPasswordHasher<UserCredential> _hasher;

        public ChangePasswordHandle(IUserCredentialRepository credRepo, IPasswordHasher<UserCredential> hasher)
        {
            _credRepo = credRepo;
            _hasher = hasher;
        }

        public async Task<ChangePasswordHandleResponse> Handle(ChangePasswordHandleRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
                return new ChangePasswordHandleResponse { Error = true, Message = "Geçersiz istek." };

            if (request.NewPassword.Length < 6)
                return new ChangePasswordHandleResponse { Error = true, Message = "Yeni parola en az 6 karakter olmalıdır." };

            var cred = await _credRepo.GetByEmailAsync(request.Email);
            if (cred is null)
                return new ChangePasswordHandleResponse { Error = true, Message = "Kullanıcı bulunamadı." };

            // Two modes: with current password (authenticated) OR with identity verification (forgot flow)
            var hasCurrent = !string.IsNullOrWhiteSpace(request.CurrentPassword);
            if (hasCurrent)
            {
                var verify = _hasher.VerifyHashedPassword(cred, cred.PasswordHash, request.CurrentPassword);
                if (verify == PasswordVerificationResult.Failed)
                    return new ChangePasswordHandleResponse { Error = true, Message = "Mevcut parola hatalı." };
            }
            else
            {
                // Identity verification
                if (string.IsNullOrWhiteSpace(request.IdentityNumber) || string.IsNullOrWhiteSpace(request.FullName))
                    return new ChangePasswordHandleResponse { Error = true, Message = "Kimlik bilgileri zorunlu." };

                var person = cred.User?.Person;
                if (person == null)
                    return new ChangePasswordHandleResponse { Error = true, Message = "Kişi bilgileri eksik." };

                if (!long.TryParse(request.IdentityNumber.Trim(), out var providedId))
                    return new ChangePasswordHandleResponse { Error = true, Message = "Kimlik numarası geçersiz." };

                var providedFullNameNorm = request.FullName.Trim().ToUpperInvariant();
                var storedFullNameNorm = (person.Name ?? string.Empty).Trim().ToUpperInvariant();

                if (person.IdentityNumber != providedId || storedFullNameNorm != providedFullNameNorm)
                    return new ChangePasswordHandleResponse { Error = true, Message = "Kimlik doğrulaması başarısız." };
            }

            cred.PasswordHash = _hasher.HashPassword(cred, request.NewPassword);
            cred.SecurityStamp = Guid.NewGuid().ToString("N");
            cred.PasswordUpdatedAt = DateTime.UtcNow;
            cred.AccessFailedCount = 0;
            cred.LockoutEnd = null;
            await _credRepo.UpdateAsync(cred);

            return new ChangePasswordHandleResponse { Error = false, Message = "Parola güncellendi." };
        }
    }
}
