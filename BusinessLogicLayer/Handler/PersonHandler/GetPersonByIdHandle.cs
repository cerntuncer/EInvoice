using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using MediatR;

namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class GetPersonByIdHandle : IRequestHandler<GetPersonByIdHandleRequest, GetPersonByIdHandleResponse>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonByIdHandle(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<GetPersonByIdHandleResponse> Handle(GetPersonByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var person = _personRepository.GetById(request.Id);

            if (person == null)
            {
                return new GetPersonByIdHandleResponse
                {
                    Error = true,
                    Message = "Kişi bulunamadı."
                };
            }

            return new GetPersonByIdHandleResponse
            {
                Error = false,
                Message = "Kişi başarıyla getirildi.",
                Id = person.Id,
                Name = person.Name,
                IdentityNumber = person.IdentityNumber,
                TaxOffice = person.TaxOffice,
                Type = person.Type,
                Status = person.Status
            };
        }
    }
}
