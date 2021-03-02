using FluentValidation;
using Indice.AspNetCore.Identity.Models;

namespace Indice.Identity.Models.Validators
{
    /// <summary>
    /// Validator for <see cref="LoginViewModel"/> model.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginInputModel>
    {
        /// <summary>
        /// Creates a new instance of <see cref="LoginInputModel"/>.
        /// </summary>
        public LoginValidator() {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please provide a username");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please provide a password");
        }
    }
}
