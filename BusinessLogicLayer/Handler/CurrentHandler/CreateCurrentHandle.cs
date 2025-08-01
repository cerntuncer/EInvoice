﻿using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class CreateCurrentHandle : IRequestHandler<CreateCurrentHandleRequest, CreateCurrentHandleResponse>
    {
        private readonly ICurrentRepository _currentRepository;
        private readonly IUserRepository _userRepository;

        public CreateCurrentHandle(ICurrentRepository currentRepository, IUserRepository userRepository)
        {
            _currentRepository = currentRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateCurrentHandleResponse> Handle(CreateCurrentHandleRequest request, CancellationToken cancellationToken)
        {
            string message = null;

            if (string.IsNullOrWhiteSpace(request.Name))
                message = "İsim boş olamaz.";

            else if (request.UserId <= 0)
                message = "Geçerli bir kullanıcı ID girilmelidir.";

            else if (_userRepository.Find(request.UserId) == null)
                message = "Kullanıcı bulunamadı.";

            if (message != null)
            {
                return new CreateCurrentHandleResponse
                {
                    Message = message,
                    Error = true
                };
            }

            var current = new Current
            {
                Name = request.Name,
                Balance = request.Balance,
                CurrencyType = request.CurrencyType,
                CurrentType = request.CurrentType,
                UserId = request.UserId,
                Status = Status.Active
            };

            _currentRepository.Add(current);

            return new CreateCurrentHandleResponse
            {
                Message = "Cari hesap başarıyla oluşturuldu.",
                Error = false
            };
        }
    }
}
