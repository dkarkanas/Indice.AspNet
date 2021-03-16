using System.Collections.Generic;

namespace Indice.AspNetCore.Fido.Models
{
    public abstract class ValidatableModel
    {
        public abstract ModelValidationResult Validate();
    }

    public class ModelValidationResult
    {
        public bool Succeeded { get; set; }
        public IList<string> Errors { get; set; }

        public static ModelValidationResult Success() => new() {
            Succeeded = true,
            Errors = new List<string>()
        };

        public static ModelValidationResult Fail() => Fail(new List<string>());

        public static ModelValidationResult Fail(IList<string> errors) => new() {
            Succeeded = false,
            Errors = errors
        };
    }
}
