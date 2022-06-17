using ScoreStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreStore.Validators
{
    public class ProfileAvailableAttribute : ValidationAttribute
    {
        public ProfileAvailableAttribute()
        {
        }

        public string GetErrorMessage(string name)
        {
            return $"Profile Name '{name}' is already in use.";
        }

        public string GetErrorMessage()
        {
            return $"Profile Name entry is invalid.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // cast input field to string
            var input = (string)value;

            // obtain list of all usernames in database
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var users = _context.Users.Select(u => new { u.Name });
            
            // filter usernames to see if any matches the input
            users = users.Where(u => u.Name.Equals(input));

            // reject inputs that already exist or are invalid
            if (users.Count() > 0)
            {
                return new ValidationResult(GetErrorMessage(input));
            } else if (String.IsNullOrEmpty(input) || input.ToLower().Equals("other"))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
