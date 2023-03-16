using Microsoft.AspNetCore.Authentication;

public class LoginViewModel
{
    public string Email{get;set;}
    public string Password{get;set;}
    public string? returnUrl{get;set;}

    public IList<AuthenticationScheme> ExternalLogins{get;set;}
}