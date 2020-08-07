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
    public class CommentsController : Controller
    {
        private readonly BouffageContext _context;

        public CommentsController(BouffageContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var bouffageContext = _context.Comment.Include(c => c.Reply);
            return View(await bouffageContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Reply)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ReplyCommentId"] = new SelectList(_context.Comment, "CommentId", "Critique");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Critique,CommentPosted,Useful,Useless,TypeId,TypeOfComments,UserCommentedId,CommentOnRecipeId,ReplyCommentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReplyCommentId"] = new SelectList(_context.Comment, "CommentId", "Critique", comment.ReplyCommentId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ReplyCommentId"] = new SelectList(_context.Comment, "CommentId", "Critique", comment.ReplyCommentId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Critique,CommentPosted,Useful,Useless,TypeId,TypeOfComments,UserCommentedId,CommentOnRecipeId,ReplyCommentId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["ReplyCommentId"] = new SelectList(_context.Comment, "CommentId", "Critique", comment.ReplyCommentId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Reply)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }


        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> LeaveAComment([FromForm] int recipe_commented, [FromForm] string critique, int? replyid, [FromForm] Comment.TypeOfC type)
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

            Comment comment = new Comment
            {
                CommentOnRecipeId = recipe_commented,
                Critique = critique,
                CommentPosted = DateTime.Now,
                TypeOfComments = type,
                UserCommentedId = Useruserid,
                Useful = 0,
                Useless = 0
            };

            if(replyid != null)
            {
                comment.ReplyCommentId = replyid;
            }

            _context.Add(comment);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());

        }
    }
}
