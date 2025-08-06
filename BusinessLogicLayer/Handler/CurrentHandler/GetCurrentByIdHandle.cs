using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class GetCurrentByIdHandle : IRequestHandler<GetCurrentByIdHandleRequest, GetCurrentByIdHandleResponse>
    {
        private readonly ICurrentRepository _currentRepository;

        public GetCurrentByIdHandle(ICurrentRepository currentRepository)
        {
            _currentRepository = currentRepository;
        }

        public async Task<GetCurrentByIdHandleResponse> Handle(GetCurrentByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var cari = _currentRepository.GetById(request.Id);

            if (cari == null)
            {
                return new GetCurrentByIdHandleResponse
                {
                    Error = true,
                    Message = "Cari hesap bulunamadı."
                };
            }

            return new GetCurrentByIdHandleResponse
            {
                Error = false,
                Message = "Cari hesap başarıyla getirildi.",
                Id = cari.Id,
                Name = cari.Name,
                Balance = cari.Balance,
                CurrencyType = cari.CurrencyType,
                CurrentType = cari.CurrentType,
                UserId = cari.UserId,
                Status = cari.Status
            };
        }
    }
}
