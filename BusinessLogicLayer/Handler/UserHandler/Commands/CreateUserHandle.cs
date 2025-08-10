using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.UserHandler.DTOs;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // ops: DbUpdateException için

namespace BusinessLogicLayer.Handler.UserHandler.Commands
{
    public class CreateUserHandle : IRequestHandler<CreateUserHandleRequest, CreateUserHandleResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly ICustomerSupplierRepository _customerSupplierRepository;
        private readonly IUserCredentialRepository _credentialRepository;
        private readonly IPasswordHasher<UserCredential> _hasher;
        private readonly IMediator _mediator;

        public CreateUserHandle(
            IUserRepository userRepository,
            IPersonRepository personRepository,
            ICustomerSupplierRepository customerSupplierRepository,
            IUserCredentialRepository credentialRepository,
            IPasswordHasher<UserCredential> hasher,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _customerSupplierRepository = customerSupplierRepository;
            _credentialRepository = credentialRepository;
            _hasher = hasher;
            _mediator = mediator;
        }

        public async Task<CreateUserHandleResponse> Handle(CreateUserHandleRequest request, CancellationToken cancellationToken)
        {
            string? message = null;
            long? personId = null;
            bool existingPersonProvided = true; // true => request.PersonId kullanıldı

            // --- Enum doğrulamaları ---
            if (!Enum.IsDefined(typeof(Status), request.Status))
                message = "Durum bilgisi geçersiz.";
            else if (!Enum.IsDefined(typeof(UserType), request.Type))
                message = "Kullanıcı tipi geçersiz.";

            // --- Email / Password doğrulama ---
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

            if (message == null && (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6))
                message = "Parola en az 6 karakter olmalıdır.";

            // --- PersonId / Person oluşturma ---
            if (message == null && request.PersonId == null)
            {
                if (request.Person == null)
                {
                    message = "PersonId ya da yeni oluşturulacak Person bilgileri iletilmelidir.";
                }
                else
                {
                    var newPerson = await _mediator.Send(request.Person, cancellationToken);
                    if (newPerson.Error == false && newPerson.Id.HasValue)
                    {
                        personId = newPerson.Id.Value;
                        existingPersonProvided = false;
                    }
                    else
                    {
                        message = newPerson.Message ?? "Kişi oluşturulamadı.";
                    }
                }
            }
            else if (message == null && _personRepository.Find(request.PersonId!.Value) == null)
            {
                message = "Gönderilen Id'ye uygun kişi bulunamadı.";
            }

            // --- Aynı kişi başka User/CustomerSupplier'a bağlı mı? ---
            if (message == null && existingPersonProvided)
            {
                var existingUser = _userRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                var existingCustomerSupplier = _customerSupplierRepository.FirstOrDefault(b => b.PersonId == request.PersonId);
                if (existingUser != null || existingCustomerSupplier != null)
                {
                    message = "Belirtilen kişi başka bir Kullanıcı/Tedarikçi/Müşteriye bağlı.";
                }
            }

            // --- Email uniq mi? ---
            if (message == null)
            {
                var existingCred = await _credentialRepository.GetByEmailAsync(request.Email);
                if (existingCred is not null)
                    message = "Bu e‑posta zaten kullanılmakta.";
            }

            if (message != null)
            {
                return new CreateUserHandleResponse { Error = true, Message = message };
            }

            if (personId == null)
                personId = request.PersonId!.Value;

            // --- User oluştur ---
            var user = new User
            {
                Type = request.Type,
                PersonId = personId.Value,
                Status = request.Status
            };
            _userRepository.Add(user);

            // --- Credential oluştur ---
            var cred = new UserCredential
            {
                // Id => int/long ise verme; Identity üretsin. Guid ise burada set et.
                // Id = Guid.NewGuid(),
                Email = request.Email.Trim(),
                Provider = "Local",
                EmailConfirmed = false,
                LockoutEnabled = request.LockoutEnabled,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,

                // En önemli kısım: aynı DbContext scope'unda navigation ile ilişkilendir.
                User = user
                // UserId'yi EF kendisi doldurur (User Identity üretince) — aynı context olması şart.
            };
            cred.PasswordHash = _hasher.HashPassword(cred, request.Password);

            await _credentialRepository.AddAsync(cred);

           

            return new CreateUserHandleResponse
            {
                Error = false,
                Message = existingPersonProvided
                    ? "Kullanıcı ve kimlik başarıyla oluşturuldu."
                    : "Kullanıcı, kişi ve kimlik başarıyla oluşturuldu.",
                Id = user.Id,                   // Identity ise Save sonrası dolar (Add kaydediyorsa)
                PersonId = personId,
                CredentialId = cred.Id,         // Identity ise Save sonrası dolar
                Email = cred.Email
            };
        }
    }
}
