using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.PersonHandler.DTOs;
using MediatR;

public class DeletePersonHandle : IRequestHandler<DeletePersonHandleRequest, DeletePersonHandleResponse>
{
    private readonly IPersonRepository _repository;

    public DeletePersonHandle(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeletePersonHandleResponse> Handle(DeletePersonHandleRequest request, CancellationToken cancellationToken)
    {
        var person = _repository.Find(request.Id);

        if (person == null)
        {
            return new DeletePersonHandleResponse
            {
                Error = true,
                Message = "Silinecek kişi bulunamadı."
            };
        }

        _repository.Delete(person);

        return new DeletePersonHandleResponse
        {
            Error = false,
            Message = "Kişi silindi."
        };
    }
}
