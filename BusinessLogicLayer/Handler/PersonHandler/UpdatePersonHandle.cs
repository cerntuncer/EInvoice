using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.PersonHandler;
using MediatR;

public class UpdatePersonHandle : IRequestHandler<UpdatePersonHandleRequest, UpdatePersonHandleResponse>
{
    private readonly IPersonRepository _repository;

    public UpdatePersonHandle(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdatePersonHandleResponse> Handle(UpdatePersonHandleRequest request, CancellationToken cancellationToken)
    {
        var person = _repository.Find(request.Id);

        if (person == null)
        {
            return new UpdatePersonHandleResponse
            {
                Error = true,
                Message = "Kişi bulunamadı."
            };
        }

        person.Name = request.Name;
        person.IdentityNumber = request.IdentityNumber;
        person.TaxOffice = request.TaxOffice;
        person.Type = request.Type;
        person.Status = request.Status;

        _repository.Update(person);

        return new UpdatePersonHandleResponse
        {
            Error = false,
            Message = "Kişi bilgileri güncellendi."
        };
    }
}
