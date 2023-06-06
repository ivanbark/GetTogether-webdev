using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Areas.Identity.Data;
using MVC.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups/5
        [HttpGet("getMembers/{id}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetMembers(int id)
        {
            if (_context.Groups == null)
            {
                return NotFound();
            }
            var group = await _context.Groups.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            var ag = await _context.ApplicationUserGroups.Where(ag => ag.GroupId == id).ToListAsync();
            var members = new List<UserDto>();
            foreach (var item in ag)
            {
                var member = await _context.Users.FindAsync(item.MemberId);
                if (member.Id != group.OwnerId)
                {
                    members.Add(new UserDto
                    {
                        Id = member.Id,
                        Name = member.Name,
                        Email = member.Email
                    });
                }
            }

            return @members;
        }

        // GET: api/Groups/5
        [HttpGet("getUsers/{groupid}/{searchstring}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(int groupid, string searchstring)
        {
            var group = await _context.Groups.FindAsync(groupid);
            var users = await _context.Users.Where(u => u.Name.Contains(searchstring) || u.Email.Contains(searchstring)).ToListAsync();
            var members = new List<UserDto>();
            foreach (var user in users)
            {
                if (group.OwnerId != user.Id && !_context.ApplicationUserGroups.Any(ag => ag.GroupId == group.Id && ag.MemberId == user.Id))
                {
                    members.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email
                    });
                }
            }
            return members;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("addMember/{userid}/{groupid}")]
        public async Task<IActionResult> PutGroup(string userid, int groupid)
        {
            var group = await _context.Groups.FindAsync(groupid);
            if (group == null)
            {
                return NotFound();
            }
            
            var ag = _context.ApplicationUserGroups.Add(new ApplicationUserGroup
            {
                GroupId = groupid,
                MemberId = userid
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(groupid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Group>> PostGroup(Group @group)
        //{
        //    if (_context.Groups == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Groups'  is null.");
        //    }
        //    _context.Groups.Add(@group);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
        //}

        // DELETE: api/Groups/5
        [HttpDelete("removeMember/{userid}/{groupid}")]
        public async Task<IActionResult> DeleteGroup(string userid, int groupid)
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var group = await _context.Groups.FindAsync(groupid);
            if (group == null)
            {
                return NotFound();
            }
            else if (group.OwnerId != loggedInUserId)
            {
                return Unauthorized();
            }
            else if (group.OwnerId == userid)
            {
                return BadRequest();
            }

            _context.ApplicationUserGroups.Remove(await _context.ApplicationUserGroups.FindAsync(groupid, userid));
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
