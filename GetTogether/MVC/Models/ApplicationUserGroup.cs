using MVC.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ApplicationUserGroup
    {
        [Key, Column(Order = 1)]
        public int GroupId { get; set; }
        [Key, Column(Order = 2)]
        public string MemberId { get; set; }
        public Group Group { get; set; }
        public ApplicationUser Member { get; set; }

    }
}
