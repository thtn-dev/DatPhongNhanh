using DatPhongNhanh.OAuth.Business.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace DatPhongNhanh.OAuth.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Route("identity/[controller]")]
public class UserController : Controller
{
    [HttpGet]
    [Route("signin")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    [Route("signin")]
    public IActionResult SignIn([FromBody] SignInDto dto)
    {
        return Ok(dto);
    }
}

