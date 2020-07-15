using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bouffage.Data;
using Bouffage.Models;
using Bouffage.ViewModels;
using System.Collections.Immutable;

namespace Bouffage.Controllers
{
    public class RecipesController : Controller
    {
        private readonly BouffageContext _context;

        public RecipesController(BouffageContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            IQueryable<Recipe> recipes = _context.Recipe.AsQueryable();
            IQueryable<Comment> comments = _context.Comment.AsQueryable();
            IQueryable<User> users = _context.User.AsQueryable();
            IQueryable<Ingredient> ingredients = _context.Ingredient.AsQueryable();

            var recipesandcomments = new RecipeAndCommentsViewModel
            {
                Recipes = await recipes.ToListAsync(),
                Comments = await comments.ToListAsync(),
                Users = await users.ToListAsync(),
                Ingredients = await ingredients.ToListAsync()
            };

            return View(recipesandcomments);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            var recipe = new Recipe();
            AddRecipe viewmodel = new AddRecipe
            {
                Recipe = recipe,
                CategoryList = new MultiSelectList(_context.Category.OrderBy(s => s.CategoryId), "Tags"),
                SelectedCategories = recipe.Categories.Select(sa => sa.CategoryId)
            };

            return View(viewmodel);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddRecipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe.Recipe);



                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Title,CuisineId,Cuisine,Essay,Preparation,PrepTime,CookTime,Servings,ComplexityId,Complexity,Upvotes,Downvotes,PostingDate,SpecialEquipment,UserPostedRecipeId")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
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
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.RecipeId == id);
        }

        // GET: Recipes
        public async Task<IActionResult> ShowThisRecipe(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FirstOrDefaultAsync(m => m.RecipeId == id);
            IQueryable<Recipe> recipes = _context.Recipe.AsQueryable();
            recipes = recipes.Where(m => m.RecipeId == id).Include(m => m.User).Include(m => m.CommentsOnThisRecipe).Include(m => m.Ingredients);
            if (recipe == null)
            {
                return NotFound();
            }

            SingleRecipeViewModel recipesandcomments = new SingleRecipeViewModel
            {
                Recipe = recipes.FirstOrDefault(m => m.RecipeId == id)
            };

            return View(recipesandcomments);
        }
    }
}
