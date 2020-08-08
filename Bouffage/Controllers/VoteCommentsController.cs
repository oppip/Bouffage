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

namespace Bouffage.Controllers
{
    public class VoteCommentsController : Controller
    {
        private readonly BouffageContext _context;

        public VoteCommentsController(BouffageContext context)
        {
            _context = context;
        }

        // GET: VoteComments
        public async Task<IActionResult> Index()
        {
            var bouffageContext = _context.VoteComment.Include(v => v.Comment).Include(v => v.User);
            return View(await bouffageContext.ToListAsync());
        }

        // GET: VoteComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteComment = await _context.VoteComment
                .Include(v => v.Comment)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VoteCommentId == id);
            if (voteComment == null)
            {
                return NotFound();
            }

            return View(voteComment);
        }

        // GET: VoteComments/Create
        public IActionResult Create()
        {
            ViewData["UserVotedThisComment"] = new SelectList(_context.Comment, "CommentId", "Critique");
            ViewData["UserVotedThisComment"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: VoteComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteCommentId,UserVotedThisComment,CommentGotVoted,UpOrDown")] VoteComment voteComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voteComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserVotedThisComment"] = new SelectList(_context.Comment, "CommentId", "Critique", voteComment.UserVotedThisComment);
            ViewData["UserVotedThisComment"] = new SelectList(_context.User, "UserId", "Email", voteComment.UserVotedThisComment);
            return View(voteComment);
        }

        // GET: VoteComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteComment = await _context.VoteComment.FindAsync(id);
            if (voteComment == null)
            {
                return NotFound();
            }
            ViewData["UserVotedThisComment"] = new SelectList(_context.Comment, "CommentId", "Critique", voteComment.UserVotedThisComment);
            ViewData["UserVotedThisComment"] = new SelectList(_context.User, "UserId", "Email", voteComment.UserVotedThisComment);
            return View(voteComment);
        }

        // POST: VoteComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoteCommentId,UserVotedThisComment,CommentGotVoted,UpOrDown")] VoteComment voteComment)
        {
            if (id != voteComment.VoteCommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voteComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteCommentExists(voteComment.VoteCommentId))
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
            ViewData["UserVotedThisComment"] = new SelectList(_context.Comment, "CommentId", "Critique", voteComment.UserVotedThisComment);
            ViewData["UserVotedThisComment"] = new SelectList(_context.User, "UserId", "Email", voteComment.UserVotedThisComment);
            return View(voteComment);
        }

        // GET: VoteComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteComment = await _context.VoteComment
                .Include(v => v.Comment)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VoteCommentId == id);
            if (voteComment == null)
            {
                return NotFound();
            }

            return View(voteComment);
        }

        // POST: VoteComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voteComment = await _context.VoteComment.FindAsync(id);
            _context.VoteComment.Remove(voteComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteCommentExists(int id)
        {
            return _context.VoteComment.Any(e => e.VoteCommentId == id);
        }


        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> VoteComment([FromForm] int comment, [FromForm] string vote)
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
            string[] v = vote.Split("&%&");
            string u_or_d = v[0];

            int userkarma = 0;
            Int32.TryParse(v[1], out userkarma);

            VoteComment vc = new VoteComment
            {
                UserVotedThisComment = Useruserid,
                CommentGotVoted = comment,
                UpOrDown = u_or_d[0]
            };
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userkarma);
            var commentkarma = await _context.Comment.FirstOrDefaultAsync(m => m.CommentId == comment);
            var hashevoted = await _context.VoteComment.FirstOrDefaultAsync(m => m.CommentGotVoted == comment && m.UserVotedThisComment == Useruserid);
            if (hashevoted == null)
            {
                if (vc.UpOrDown == 'u')
                {
                    user.Karma += 1;
                    commentkarma.Useful += 1;
                }
                else
                {
                    user.Karma -= 1;
                    commentkarma.Useless += 1;
                }
                _context.Add(vc);
            }
            else
            {
                if (hashevoted.UpOrDown == 'u')
                {
                    if (vc.UpOrDown == 'u')
                    {
                        user.Karma -= 1;
                        commentkarma.Useful -= 1;
                        _context.VoteComment.Remove(hashevoted);
                    }
                    else
                    {
                        user.Karma -= 2;
                        commentkarma.Useful -= 1;
                        commentkarma.Useless += 1;
                        hashevoted.UpOrDown = 'd';
                        _context.VoteComment.Update(hashevoted);
                    }
                }
                else
                {
                    if (vc.UpOrDown == 'u')
                    {
                        user.Karma += 2;
                        commentkarma.Useful += 1;
                        commentkarma.Useless -= 1;
                        hashevoted.UpOrDown = 'u';
                        _context.VoteComment.Update(hashevoted);
                    }
                    else
                    {
                        user.Karma += 1;
                        commentkarma.Useless -= 1;
                        _context.VoteComment.Remove(hashevoted);
                    }
                }
            }
            _context.Update(user);
            _context.Update(commentkarma);
            await _context.SaveChangesAsync();
            if (v.Length == 3)
            {
                return Redirect(Request.Headers["Referer"].ToString() + "#" + v[2]);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }


        }


    }
}
