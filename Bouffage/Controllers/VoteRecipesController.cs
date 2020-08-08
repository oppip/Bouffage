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
using System.Security.Policy;

namespace Bouffage.Controllers
{
    public class VoteRecipesController : Controller
    {
        private readonly BouffageContext _context;

        public VoteRecipesController(BouffageContext context)
        {
            _context = context;
        }

        // GET: VoteRecipes
        public async Task<IActionResult> Index()
        {
            var bouffageContext = _context.VoteRecipe.Include(v => v.Recipe).Include(v => v.User);
            return View(await bouffageContext.ToListAsync());
        }

        // GET: VoteRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteRecipe = await _context.VoteRecipe
                .Include(v => v.Recipe)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VoteRecipeId == id);
            if (voteRecipe == null)
            {
                return NotFound();
            }

            return View(voteRecipe);
        }

        // GET: VoteRecipes/Create
        public IActionResult Create()
        {
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.Recipe, "RecipeId", "Title");
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: VoteRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoteRecipeId,UserVotedThisRecipe,RecipeGotVoted,UpOrDown")] VoteRecipe voteRecipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voteRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.Recipe, "RecipeId", "Title", voteRecipe.UserVotedThisRecipe);
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.User, "UserId", "Email", voteRecipe.UserVotedThisRecipe);
            return View(voteRecipe);
        }

        // GET: VoteRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteRecipe = await _context.VoteRecipe.FindAsync(id);
            if (voteRecipe == null)
            {
                return NotFound();
            }
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.Recipe, "RecipeId", "Title", voteRecipe.UserVotedThisRecipe);
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.User, "UserId", "Email", voteRecipe.UserVotedThisRecipe);
            return View(voteRecipe);
        }

        // POST: VoteRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoteRecipeId,UserVotedThisRecipe,RecipeGotVoted,UpOrDown")] VoteRecipe voteRecipe)
        {
            if (id != voteRecipe.VoteRecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voteRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteRecipeExists(voteRecipe.VoteRecipeId))
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
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.Recipe, "RecipeId", "Title", voteRecipe.UserVotedThisRecipe);
            ViewData["UserVotedThisRecipe"] = new SelectList(_context.User, "UserId", "Email", voteRecipe.UserVotedThisRecipe);
            return View(voteRecipe);
        }

        // GET: VoteRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteRecipe = await _context.VoteRecipe
                .Include(v => v.Recipe)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.VoteRecipeId == id);
            if (voteRecipe == null)
            {
                return NotFound();
            }

            return View(voteRecipe);
        }

        // POST: VoteRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voteRecipe = await _context.VoteRecipe.FindAsync(id);
            _context.VoteRecipe.Remove(voteRecipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteRecipeExists(int id)
        {
            return _context.VoteRecipe.Any(e => e.VoteRecipeId == id);
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> VoteRecipe([FromForm] int recipe, [FromForm] string vote)
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

            VoteRecipe vr = new VoteRecipe
            {
                RecipeGotVoted = recipe,
                UserVotedThisRecipe = Useruserid,
                UpOrDown = u_or_d[0]
            };
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userkarma);
            var recipekarma = await _context.Recipe.FirstOrDefaultAsync(m => m.RecipeId == recipe);
            var hashevoted = await _context.VoteRecipe.FirstOrDefaultAsync(m => m.RecipeGotVoted == recipe && m.UserVotedThisRecipe == Useruserid);
            if (hashevoted == null)
            {
                if (vr.UpOrDown == 'u')
                {
                    user.Karma += 1;
                    recipekarma.Upvotes += 1;
                }
                else
                {
                    user.Karma -= 1;
                    recipekarma.Downvotes += 1;
                }
                _context.Add(vr);
            }
            else
            {
                if(hashevoted.UpOrDown == 'u')
                {
                    if (vr.UpOrDown == 'u')
                    {
                        user.Karma -= 1;
                        recipekarma.Upvotes -= 1;
                        _context.VoteRecipe.Remove(hashevoted);
                    }
                    else
                    {
                        user.Karma -= 2;
                        recipekarma.Upvotes -= 1;
                        recipekarma.Downvotes += 1;
                        hashevoted.UpOrDown = 'd';
                        _context.VoteRecipe.Update(hashevoted);
                    }
                }
                else
                {
                    if (vr.UpOrDown == 'u')
                    {
                        user.Karma += 2;
                        recipekarma.Upvotes += 1;
                        recipekarma.Downvotes -= 1;
                        hashevoted.UpOrDown = 'u';
                        _context.VoteRecipe.Update(hashevoted);                        
                    }
                    else
                    {
                        user.Karma += 1;
                        recipekarma.Downvotes -= 1;
                        _context.VoteRecipe.Remove(hashevoted);
                    }
                }
            }
            _context.Update(user);
            _context.Update(recipekarma);
            await _context.SaveChangesAsync();
            if (v.Length == 3)
            {
                return Redirect(Request.Headers["Referer"].ToString() + "#"+v[2]);
            }
            else
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            

        }
    }
}
