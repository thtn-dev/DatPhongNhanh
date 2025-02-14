using Asp.Versioning;
using DatPhongNhanh.BusinessLogic.Services.Interfaces;
using DatPhongNhanh.Data.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DatPhongNhanh.WebApiV1.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("GetById/{userId}")]
        [Authorize(AuthenticationSchemes = "GoogleAuth")]
        public async Task<IActionResult> GetById(long userId)
        {
            return Ok(await _userService.GetUserByIdAsync(userId));
        }

        [HttpGet]
        [Route("GetByName")]

        public async Task<IActionResult> GetByName([FromQuery] string userName)
        {
            return Ok(await _userService.GetUserByNameAsync(userName));
        }

        [HttpPost]
        [Route("Create")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(bool))]
        public async Task<IActionResult> CreateUser([FromBody] UserEntity user)
        {
            return Ok(await _userService.CreateUserAsync(user));
        }
    }
}
