using ScoreStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Validators
{
    /* Custom validaton attribute for validating unique profile names upon user registration.
     * 
     * referencing: 
     * https://stackoverflow.com/questions/68591266/how-to-validate-unique-property-in-asp-net-core 
     */
    public class ProfileUniqueAttribute : ValidationAttribute
    {
        private readonly string _EmailProperty;
        public ProfileUniqueAttribute(string EmailProperty)
        {
            _EmailProperty = EmailProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // store user's input to string
            string input = value.ToString();

            // validate input does not equal to placeholder username of 'other'
            if (input.ToLower().Equals("other"))
            {
                return new ValidationResult(GetErrorMessage(input));
            }

            // obtain this user's email property
            var property = validationContext.ObjectType.GetProperty(_EmailProperty);
            
            // if user exists...
            if (property != null)
            {
                // obtain user's email as lowercase string
                var emailValue = property.GetValue(validationContext.ObjectInstance, null).ToString().ToLower();
                
                // check for any other users that have this profile name
                var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
                var entity = _context.Users.SingleOrDefault(e => e.Name == input && e.Email.ToLower() != emailValue);

                // other user was found with this profile name
                if (entity != null)
                {
                    return new ValidationResult(GetErrorMessage(input));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string name)
        {
            return $"Profile Name '{name}' is already in use.";
        }
    }
}
