using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    public AdminController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager)
    {
       this.roleManager=roleManager;
       this.userManager=userManager;
       this.signInManager=signInManager;
    }
    public IActionResult CreateRole()
    {
         return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateRole(AdminViewModel model)
    {
        var existing=await roleManager.FindByNameAsync(model.RoleName);
        IdentityResult result=null;
        if(existing==null){
 IdentityRole role=new IdentityRole{
            Name=model.RoleName
        };
        result=await roleManager.CreateAsync(role);
        }
       
        
        var user=(await userManager.FindByEmailAsync(model.UserName));
        if(((result!=null && result.Succeeded)||(existing!=null)) && user!=null)
        {
           var resultUser=await userManager.AddToRoleAsync( user,model.RoleName);
           if(resultUser.Succeeded)
           {
            await signInManager.SignInAsync(user,false,null);
           }
        }
        return Redirect("/Gift/GetGiftItems");
    }

    public IActionResult DeleteRole()
    {
        
        return Ok();
    }
    public IActionResult UpdateRole()
    {
        return Ok();
    }
    public string getAllUser(object dom)
    {
        return null;
    }
}