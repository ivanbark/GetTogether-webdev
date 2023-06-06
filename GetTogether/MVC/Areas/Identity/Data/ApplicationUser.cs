using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVC.Models;

namespace MVC.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    [InverseProperty("Owner")]
    public virtual ICollection<Group> IsOwnerOfGroups { get; }
    public virtual ICollection<ApplicationUserGroup> Groups { get; }

    public static explicit operator ApplicationUser(Task<ApplicationUser?> v)
    {
        throw new NotImplementedException();
    }

    //TODO
    // get user id
    // get user name
    // get user email
    // get user
    // get user groups
}

