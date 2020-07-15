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
    public class RecipeInCategoriesController : Controller
    {
        private readonly BouffageContext _context;

        public RecipeInCategoriesController(BouffageContext context)
        {
            _context = context;
        }

        // GET: RecipeInCategories
        public async Task<IActionResult> Index()
        {
            var bouffageContext = _context.RecipeInCategory.Include(r => r.Category).Include(r => r.Recipe);
            return View(await bouffageContext.ToListAsync());
        }

        // GET: RecipeInCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeInCategory = await _context.RecipeInCategory
                .Include(r => r.Category)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeInCategoryId == id);
            if (recipeInCategory == null)
            {
                return NotFound();
            }

            return View(recipeInCategory);
        }

        // GET: RecipeInCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "RecipeId", "Title");
            return View();
        }

        // POST: RecipeInCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeInCategoryId,RecipeId,CategoryId")] RecipeInCategory recipeInCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeInCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", recipeInCategory.CategoryId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "RecipeId", "Title", recipeInCategory.RecipeId);
            return View(recipeInCategory);
        }

        // GET: RecipeInCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeInCategory = await _context.RecipeInCategory.FindAsync(id);
            if (recipeInCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", recipeInCategory.CategoryId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "RecipeId", "Title", recipeInCategory.RecipeId);
            return View(recipeInCategory);
        }

        // POST: RecipeInCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeInCategoryId,RecipeId,CategoryId")] RecipeInCategory recipeInCategory)
        {
            if (id != recipeInCategory.RecipeInCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeInCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeInCategoryExists(recipeInCategory.RecipeInCategoryId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", recipeInCategory.CategoryId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "RecipeId", "Title", recipeInCategory.RecipeId);
            return View(recipeInCategory);
        }

        // GET: RecipeInCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeInCategory = await _context.RecipeInCategory
                .Include(r => r.Category)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeInCategoryId == id);
            if (recipeInCategory == null)
            {
                return NotFound();
            }

            return View(recipeInCategory);
        }

        // POST: RecipeInCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeInCategory = await _context.RecipeInCategory.FindAsync(id);
            _context.RecipeInCategory.Remove(recipeInCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeInCategoryExists(int id)
        {
            return _context.RecipeInCategory.Any(e => e.RecipeInCategoryId == id);
        }
    }
}
