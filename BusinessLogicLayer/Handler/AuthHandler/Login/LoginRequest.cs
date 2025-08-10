namespace BusinessLogicLayer.Handler.AuthHandler.Login
{
    public sealed class LoginRequest /*: IRequest<LoginResponse>  (MediatR kullanıyorsan aç)*/
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
