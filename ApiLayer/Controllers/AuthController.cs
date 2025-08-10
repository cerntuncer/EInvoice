using System.Linq;
using System.Threading.Tasks;
using BusinessLogicLayer.DesignPatterns.Services.Auth;
using BusinessLogicLayer.Handler.AuthHandler.Login; // LoginRequest, LoginResponse
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("[controller]")] // -> /Auth
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) => _auth = auth;

        // POST: /Auth/Login
        [HttpPost("Login", Name = "AuthLogin")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            try
            {
                var result = await _auth.LoginAsync(request.Email, request.Password);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: /Auth/Refresh
        [HttpPost("Refresh", Name = "AuthRefresh")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            try
            {
                var result = await _auth.RefreshAsync(refreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: /Auth/Me
        [HttpGet("Me", Name = "AuthMe")]
        [Authorize]
        public IActionResult Me()
        {
            var sub = User.FindFirst("sub")?.Value;
            var name = User.Identity?.Name;
            var roles = User.Claims
                            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                            .Select(c => c.Value);
            return Ok(new { sub, name, roles });
        }
    }
}
