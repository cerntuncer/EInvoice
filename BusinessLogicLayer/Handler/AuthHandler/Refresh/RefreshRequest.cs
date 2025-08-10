namespace BusinessLogicLayer.Handler.AuthHandler.Refresh
{
    public sealed class RefreshRequest /*: IRequest<LoginResponse>*/
    {
        public string RefreshToken { get; set; } = null!;
    }
}
