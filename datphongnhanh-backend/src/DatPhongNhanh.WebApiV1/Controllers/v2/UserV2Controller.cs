using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatPhongNhanh.WebApiV1.Controllers.v2
{
    [Route("api/v{version:apiVersion}/xyz")]
    [ApiController]
    [ApiVersion("2.0")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("GetById/{userId}")]
        public async Task<IActionResult> GetById(long userId)
        {
            return Ok(1);
        }
    }
}
