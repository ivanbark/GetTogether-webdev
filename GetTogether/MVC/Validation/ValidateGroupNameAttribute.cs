using MVC.Areas.Identity.Data;
using MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace MVC.Validation
{
    public class ValidateGroupNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var group = (Group)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var groupInDb = _context.Groups.SingleOrDefault(g => g.Name == group.Name);
            if (groupInDb != null)
            {
                return new ValidationResult("Groupname already exists");
            }
            return ValidationResult.Success;
        }
    }
}
