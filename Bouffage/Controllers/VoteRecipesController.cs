using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bouffage.Data;
using Bouffage.Models;

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
    }
}
