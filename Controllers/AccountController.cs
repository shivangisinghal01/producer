using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private UserManager<IdentityUser> userManager;
    private SignInManager<IdentityUser> signInManager;
    public AccountController(UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager)
    {
       this.userManager=userManager;
       this.signInManager=signInManager;
    }
    [AllowAnonymous]
    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
    {
       if(ModelState.IsValid) 
       {
            IdentityUser user=new IdentityUser{
               UserName=model.Email,
               Email=model.Email,
               PasswordHash=model.Password,
               PhoneNumber=model.PhoneNumber.ToString()
            };
            var result=await userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            {
                await signInManager.SignInAsync(user,false,null);
                return Redirect("/Gift/GetGiftItems");
            }
            foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
       }
        return View(model);
    }
    [AllowAnonymous]
   public async Task<IActionResult> LogoutUser()
  {
        if(signInManager.IsSignedIn(User))
        {
            await signInManager.SignOutAsync();
            return Redirect("/Gift/GetGiftItems");
        }
        return Ok();
  }
  [AllowAnonymous]
  [Route("/Account/Login/")]
  [HttpPost]
    public  async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
    {
        
       var result= await signInManager.PasswordSignInAsync(model.Email,model.Password,false,false);
        if(result.Succeeded)
        {
            if(String.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/Gift/GetGiftItems");
            }
             return Redirect(model.returnUrl.ToString());

        }
       return Ok();
       
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Login()
    {
        var returnurl=HttpContext.Request.Query["ReturnUrl"];
        var externalLogins=await signInManager.GetExternalAuthenticationSchemesAsync();
        return View("LoginUser",new LoginViewModel{returnUrl=returnurl,ExternalLogins=externalLogins.ToList()});
    }
    //Remote Validation
    [AcceptVerbs("Get","Post")]
    public async Task<IActionResult> IsEmailInUse(string email)
    {
        var user=await userManager.FindByEmailAsync(email);
        if(user==null)
        {
            return Json(true);
        }
        return Json($"Email {email} is already in use.");
    }


    
    #region - External Login Providers
    [HttpPost]
    public IActionResult ExternalLogin(string provider,string returnUrl)
    {
        var redirectUrl=Url.Action("ExternalLoginCallback","Account",new {returnUrl=returnUrl});
        var properties=signInManager.ConfigureExternalAuthenticationProperties(provider,redirectUrl);
        return new ChallengeResult(provider,properties);
      return Ok();
    }
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null,string remoteError=null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");

    LoginViewModel loginViewModel = new LoginViewModel
    {
        returnUrl = returnUrl,
        ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
    };

    if (remoteError != null)
    {
        ModelState
            .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

        return View("Login", loginViewModel);
    }

    // Get the login information about the user from the external login provider
    var info = await signInManager.GetExternalLoginInfoAsync();
    if (info == null)
    {
        ModelState
            .AddModelError(string.Empty, "Error loading external login information.");

        return View("Login", loginViewModel);
    }

    // If the user already has a login (i.e if there is a record in AspNetUserLogins
    // table) then sign-in the user with this external login provider
    var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
        info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

    if (signInResult.Succeeded)
    {
        return LocalRedirect(returnUrl);
    }
    // If there is no record in AspNetUserLogins table, the user may not have
    // a local account
    else
    {
        // Get the email claim value
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (email != null)
        {
            // Create a new user without password if we do not have a user already
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                await userManager.CreateAsync(user);
            }

            // Add a login (i.e insert a row for the user in AspNetUserLogins table)
            await userManager.AddLoginAsync(user, info);
            await signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(returnUrl);
        }

        // If we cannot find the user email we cannot continue
  

        return View("/Gift/GetGiftItems");
    }
    }
    #endregion
}