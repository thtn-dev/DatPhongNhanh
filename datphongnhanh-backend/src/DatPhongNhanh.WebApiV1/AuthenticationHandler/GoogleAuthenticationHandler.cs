using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace DatPhongNhanh.WebApiV1.AuthenticationHandler
{
    public class GoogleAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly HttpClient _httpClient;

        public GoogleAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, HttpClient httpClient) : base(options, logger, encoder)
        {
            _httpClient = httpClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string authorizationHeader = Request.Headers["Authorization"];
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            try
            {
                // Gọi API của Google để xác thực token
                var response = await _httpClient.GetAsync($"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={token}");
                if (!response.IsSuccessStatusCode)
                {
                    return AuthenticateResult.Fail("Invalid Google Token");
                }
                var res = await response.Content.ReadAsStringAsync();

                var payload = JsonConvert.DeserializeObject<GoogleTokenValidationResponse>(res) ?? throw new Exception("Invalid payload");

                // Tạo Claims từ token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, payload.UserId),
                    new Claim(ClaimTypes.Email, payload.Email)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
            }
        }
    }

    public class GoogleTokenValidationResponse
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
