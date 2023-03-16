using System.ComponentModel.DataAnnotations;

// Custom Validation
public class EmailDomainValidation : ValidationAttribute
{
    private readonly string allowedDomain;
    public EmailDomainValidation(string allowedDomain)
    {
        this.allowedDomain=allowedDomain;
    }
    public override bool IsValid(object? value)
    {
        string[] strings = value.ToString().Split('@');
        return strings[1].ToUpper() == allowedDomain.ToUpper();
    }
}