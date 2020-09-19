using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElixinBackend.Users.UseCases.AuthenticateUseCase
{
    public class AuthenticateUser : IValidatableObject
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            return results;
        }
    }
}