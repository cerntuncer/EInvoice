using System;
using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.Handler.PersonHandler.DTOs; // mevcut Person create DTO'n nerede ise
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public sealed class CreateUserHandleRequest : IRequest<CreateUserHandleResponse>
    {
        public long? PersonId { get; set; }
        public CreatePersonHandleRequest? Person { get; set; } // senin mevcut request tipin

        [Required]
        public Status Status { get; set; }

        [Required]
        public UserType Type { get; set; } // senin enum'un

        // --- Credential alanları ---
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        public bool LockoutEnabled { get; set; } = false;
    }
}
