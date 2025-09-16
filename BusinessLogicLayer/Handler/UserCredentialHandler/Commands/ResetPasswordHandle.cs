using System.Threading;
using System.Threading.Tasks;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserCredentialHandler.DTOs;
using DatabaseAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.Commands
{
    public sealed class ResetPasswordHandle : IRequestHandler<ResetPasswordHandleRequest, ResetPasswordHandleResponse>
    {
        private readonly IUserCredentialRepository _credentialRepository;
        private readonly IPasswordHasher<UserCredential> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        public ResetPasswordHandle(
            IUserCredentialRepository credentialRepository,
            IPasswordHasher<UserCredential> passwordHasher,
            IUserRepository userRepository,
            IPersonRepository personRepository)
        {
            _credentialRepository = credentialRepository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _personRepository = personRepository;
        }

        public async Task<ResetPasswordHandleResponse> Handle(ResetPasswordHandleRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.IdentityNumber) || string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.NewPassword))
                return new ResetPasswordHandleResponse { Error = true, Message = "Geçersiz istek." };

            if (request.NewPassword.Length < 6)
                return new ResetPasswordHandleResponse { Error = true, Message = "Yeni parola en az 6 karakter olmalıdır." };

            var credential = await _credentialRepository.GetByEmailAsync(request.Email);
            if (credential is null)
                return new ResetPasswordHandleResponse { Error = true, Message = "Kullanıcı bulunamadı." };

            // Load person via User -> Person
            var user = credential.User;
            if (user == null)
                return new ResetPasswordHandleResponse { Error = true, Message = "Kullanıcı bilgileri eksik." };

            var person = user.Person;
            if (person == null)
                return new ResetPasswordHandleResponse { Error = true, Message = "Kişi bilgileri eksik." };

            // Validate identity number and full name (case-insensitive, trimmed)
            var providedIdentityTrimmed = request.IdentityNumber.Trim();
            if (!long.TryParse(providedIdentityTrimmed, out var providedIdentityAsLong))
                return new ResetPasswordHandleResponse { Error = true, Message = "Kimlik numarası geçersiz." };

            var providedFullNameNorm = request.FullName.Trim().ToUpperInvariant();
            var storedFullNameNorm = (person.Name ?? string.Empty).Trim().ToUpperInvariant();

            if (person.IdentityNumber != providedIdentityAsLong || storedFullNameNorm != providedFullNameNorm)
                return new ResetPasswordHandleResponse { Error = true, Message = "Kimlik doğrulaması başarısız." };

            // Update password hash and security stamp
            credential.PasswordHash = _passwordHasher.HashPassword(credential, request.NewPassword);
            credential.SecurityStamp = Guid.NewGuid().ToString("N");
            credential.PasswordUpdatedAt = DateTime.UtcNow;
            credential.AccessFailedCount = 0;
            credential.LockoutEnd = null;

            await _credentialRepository.UpdateAsync(credential);

            return new ResetPasswordHandleResponse { Error = false, Message = "Parola başarıyla güncellendi." };
        }
    }
}

