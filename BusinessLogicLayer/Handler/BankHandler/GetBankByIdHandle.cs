using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Handler.BankHandler
{
    public class GetBankByIdHandle : IRequestHandler<GetBankByIdHandleRequest, GetBankByIdHandleResponse>
    {
        private readonly IBankRepository _bankRepository;

        public GetBankByIdHandle(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<GetBankByIdHandleResponse> Handle(GetBankByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var bank = _bankRepository.GetById(request.Id);

            if (bank == null)
            {
                return new GetBankByIdHandleResponse
                {
                    Error = true,
                    Message = "Banka bulunamadı."
                };
            }

            return new GetBankByIdHandleResponse
            {
                Error = false,
                Message = "Banka getirildi.",
                Id = bank.Id,
                Name = bank.Name,
                Iban = bank.Iban,
                BranchCode = bank.BranchCode,
                AccountNo = bank.AccountNo,
                CurrentId = bank.CurrentId,
                Status = bank.Status
            };
        }
    }

}
