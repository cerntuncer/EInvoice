namespace BusinessLogicLayer.Handler.AuthHandler.Login
{
    public sealed class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public System.DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; } = null!;
        public System.DateTime RefreshTokenExpiresAt { get; set; }
    }
}
