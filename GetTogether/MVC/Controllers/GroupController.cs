using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Areas.Identity.Data;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult AddGroup()
        {
            return View();
        }

        [Authorize]
        public IActionResult GroupDetails(int id)
        {
            var group = _context.Groups.Find(id);
            var ag = _context.ApplicationUserGroups.Where(ag => ag.GroupId == id);
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            if (group.OwnerId == UserId) { return View(group); }
            else if (ag != null && ag.Any(ag => ag.MemberId == UserId)) { return View(group); }

            return RedirectToAction("Main", "Home");
        }

        [Authorize]
        public IActionResult AddParticipants(int id)
        {
            var group = _context.Groups.Find(id);
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (group.OwnerId == UserId) { return View(group); }

            return RedirectToAction("Main", "Home");
        }

        [HttpPost]
        public IActionResult AddGroup(Group group)
        {
            _context.ApplicationUserGroups.Add(new ApplicationUserGroup
            {
                Group = group,
                Member = _context.Users.Find(User.FindFirstValue(ClaimTypes.NameIdentifier))
            });

            ModelState.Remove("Owner");
            ModelState.Remove("Members");

            if (ModelState.IsValid)
            {
                _context.Groups.Add(group);
                _context.SaveChanges();
                return RedirectToAction("Main", "Home");
            }

            return View();
        }
    }
}
