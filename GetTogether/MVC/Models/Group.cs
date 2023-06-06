using MVC.Areas.Identity.Data;
using MVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a groupname")]
        [StringLength(25, ErrorMessage = "The groupname can only have 25 characters.")]
        [ValidateGroupName]
        public string Name { get; set; }
        public string? OwnerId { get; set; }
        [Required(ErrorMessage = "Please enter a groupdescription")]
        public string Description { get; set; }
       
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<ApplicationUserGroup> Members { get; }
    }
}
