using DatPhongNhanh.OAuth.Business.Dtos.User;
using DatPhongNhanh.OAuth.Business.Regex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatPhongNhanh.OAuth.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Route("identity", Name = "IdentityArea")]
public class IndexController: Controller
{
    private readonly ILogger<IndexController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly IUserStore<ApplicationUser> _userStore;

    public IndexController(IServiceProvider sp) 
    {
        _userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        _userStore = sp.GetRequiredService<IUserStore<ApplicationUser>>();
        _emailStore = GetEmailStore();
        _signInManager = sp.GetRequiredService<SignInManager<ApplicationUser>>();
        _logger = sp.GetRequiredService<ILogger<IndexController>>();
    }
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
    [HttpGet]
    [Route("signin", Name = "SignIn")]
    public async Task<IActionResult> SignIn([FromQuery] string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        return View();
    }

    [HttpPost]
    [Route("signin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto, [FromQuery] string? returnUrl = null)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var isEmail = UserRegex.EmailRegex().Match(dto.EmailOrUsername);
        
        var user = isEmail.Success
            ? await _userManager.FindByEmailAsync(dto.EmailOrUsername)
            : await _userManager.FindByNameAsync(dto.EmailOrUsername);
        
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("User logged in.");
        return Ok(returnUrl ?? Url.Content("~/"));
    }
    
    [HttpGet]
    [Route("signup", Name = "SignUp")]
    public IActionResult SignUp([FromQuery] string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
        return View();
    }
    
    [HttpPost]
    [Route("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto dto, [FromQuery] string? returnUrl = null)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); 
        
        var user = CreateUser();
        await _userStore.SetUserNameAsync(user, dto.Username, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, dto.Email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }
        
        await _signInManager.SignInAsync(user, false);
        return Ok( returnUrl ?? Url.Content("~/"));
    }
    private static ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                                                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}

