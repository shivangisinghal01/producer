using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class RegisterUserViewModel
{
   
    [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$",ErrorMessage ="Email is in incorrect format")]
    [Required]
    [Remote(action:"IsEmailInUse" , controller:"Account")] //Remote Validation
    [EmailDomainValidation(allowedDomain:"gmail.com",ErrorMessage ="Email must be domain gmail.com")] //Custom Vlaidation
    public string Email{get;set;}
    [MaxLength(20)]
    [Required]
    public string Password{get;set;}
    [MaxLength(10)]
    [Required]
    public string PhoneNumber{get;set;}
}