using FluentValidation;
using ECommerceApi.Models;

namespace ECommerceApi.Validators
{
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PasswordHash).NotEmpty().MinimumLength(6);
    }
}
}
