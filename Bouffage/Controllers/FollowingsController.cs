using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bouffage.Data;
using Bouffage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CodeStyle;

namespace Bouffage.Controllers
{
    public class FollowingsController : Controller
    {
        private readonly BouffageContext _context;

        public FollowingsController(BouffageContext context)
        {
            _context = context;
        }

        // GET: Followings
        public async Task<IActionResult> Index()
        {
            var bouffageContext = _context.Following.Include(f => f.UserFollowee).Include(f => f.UserFollowing);
            return View(await bouffageContext.ToListAsync());
        }

        // GET: Followings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var following = await _context.Following
                .Include(f => f.UserFollowee)
                .Include(f => f.UserFollowing)
                .FirstOrDefaultAsync(m => m.FollowingId == id);
            if (following == null)
            {
                return NotFound();
            }

            return View(following);
        }

        // GET: Followings/Create
        public IActionResult Create()
        {
            ViewData["UserFolloweeId"] = new SelectList(_context.User, "UserId", "Email");
            ViewData["UserFollowingId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: Followings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowingId,UserFollowingId,UserFolloweeId,DateFollowed")] Following following)
        {
            if (ModelState.IsValid)
            {
                _context.Add(following);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFolloweeId"] = new SelectList(_context.User, "UserId", "Email", following.UserFolloweeId);
            ViewData["UserFollowingId"] = new SelectList(_context.User, "UserId", "Email", following.UserFollowingId);
            return View(following);
        }

        // GET: Followings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var following = await _context.Following.FindAsync(id);
            if (following == null)
            {
                return NotFound();
            }
            ViewData["UserFolloweeId"] = new SelectList(_context.User, "UserId", "Email", following.UserFolloweeId);
            ViewData["UserFollowingId"] = new SelectList(_context.User, "UserId", "Email", following.UserFollowingId);
            return View(following);
        }

        // POST: Followings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FollowingId,UserFollowingId,UserFolloweeId,DateFollowed")] Following following)
        {
            if (id != following.FollowingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(following);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowingExists(following.FollowingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserFolloweeId"] = new SelectList(_context.User, "UserId", "Email", following.UserFolloweeId);
            ViewData["UserFollowingId"] = new SelectList(_context.User, "UserId", "Email", following.UserFollowingId);
            return View(following);
        }

        // GET: Followings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var following = await _context.Following
                .Include(f => f.UserFollowee)
                .Include(f => f.UserFollowing)
                .FirstOrDefaultAsync(m => m.FollowingId == id);
            if (following == null)
            {
                return NotFound();
            }

            return View(following);
        }

        // POST: Followings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var following = await _context.Following.FindAsync(id);
            _context.Following.Remove(following);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowingExists(int id)
        {
            return _context.Following.Any(e => e.FollowingId == id);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Follow([FromForm] int user_id)
        {
            var cookie = Request.Cookies["MyCookie"];
            string[] list = { "", "", "" };
            if (cookie != null)
            {
                list = cookie.Split("&%&");
            }

            int Useruserid = 0;
            Int32.TryParse(list[0], out Useruserid);
            var Userusername = list[1];
            var UserRole = list[2];

            Following follow = new Following
            {
                UserFollowingId = Useruserid,
                UserFolloweeId = user_id,
                DateFollowed = DateTime.UtcNow
            };
            var following = await _context.User.FirstOrDefaultAsync(m => m.UserId == Useruserid);
            var followed = await _context.User.FirstOrDefaultAsync(m => m.UserId == user_id);
            following.Following += 1;
            followed.Followers += 1;

            _context.Update(following);
            _context.Update(followed);
            _context.Add(follow);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Unfollow([FromForm] int user_id)
        {
            var cookie = Request.Cookies["MyCookie"];
            string[] list = { "", "", "" };
            if (cookie != null)
            {
                list = cookie.Split("&%&");
            }

            int Useruserid = 0;
            Int32.TryParse(list[0], out Useruserid);
            var Userusername = list[1];
            var UserRole = list[2];

            Following removefollowing = _context.Following.FirstOrDefault(m => m.UserFolloweeId == user_id && m.UserFollowingId == Useruserid);
            if(removefollowing == null)
            {
                return Ok("nop");
            }
            else{

                var unfollowing = await _context.User.FirstOrDefaultAsync(m => m.UserId == Useruserid);
                var unfollowed = await _context.User.FirstOrDefaultAsync(m => m.UserId == user_id);
                unfollowing.Following -= 1;
                unfollowed.Followers -= 1;

                _context.Update(unfollowing);
                _context.Update(unfollowed);

                _context.Following.Remove(removefollowing);
                await _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            
        }
    }
}
