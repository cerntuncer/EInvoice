using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateCurrentHandle : IRequestHandler<UpdateCurrentHandleRequest, UpdateCurrentHandleResponse>
{
    private readonly ICurrentRepository _currentRepository;
    private readonly IUserRepository _userRepository;

    public UpdateCurrentHandle(ICurrentRepository currentRepository, IUserRepository userRepository)
    {
        _currentRepository = currentRepository;
        _userRepository = userRepository;
    }

    public async Task<UpdateCurrentHandleResponse> Handle(UpdateCurrentHandleRequest request, CancellationToken cancellationToken)
    {
        string message = null;

        var existing = _currentRepository.Find(request.Id);
        if (existing == null)
            message = "Cari hesap bulunamadı.";

        else if (string.IsNullOrWhiteSpace(request.Name))
            message = "İsim boş olamaz.";

        else if (!_userRepository.Any(u => u.Id == request.UserId))
            message = "Geçerli bir kullanıcı bulunamadı.";

        else if (!Enum.IsDefined(typeof(CurrencyType), request.CurrencyType))
            message = "Geçersiz para birimi tipi.";

        else if (!Enum.IsDefined(typeof(CurrentType), request.CurrentType))
            message = "Geçersiz cari tipi.";

        else if (!Enum.IsDefined(typeof(Status), request.Status))
            message = "Geçersiz durum bilgisi.";

        if (message != null)
        {
            return new UpdateCurrentHandleResponse
            {
                Error = true,
                Message = message
            };
        }

        // Güncelleme işlemi
        existing.Name = request.Name;
        existing.UserId = request.UserId;
        existing.Balance = request.Balance;
        existing.CurrencyType = request.CurrencyType;
        existing.CurrentType = request.CurrentType;
        existing.Status = request.Status;

        _currentRepository.Update(existing);

        return new UpdateCurrentHandleResponse
        {
            Error = false,
            Message = "Cari hesap başarıyla güncellendi."
        };
    }
}
