using Asp.Versioning;
using DatPhongNhanh.BusinessLogic.User.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DatPhongNhanh.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserLoginDto data)
        {
            return BadRequest(data);
        }
    }
}
