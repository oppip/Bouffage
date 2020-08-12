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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Bouffage.Controllers
{
    public class RecipesController : Controller
    {
        private readonly BouffageContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment webHostEnvironment;

        public RecipesController(BouffageContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            //IQueryable<Recipe> recipes = _context.Recipe.AsQueryable();
            IQueryable<Comment> comments = _context.Comment.AsQueryable();
            IQueryable<User> users = _context.User.AsQueryable();
            IQueryable<Ingredient> ingredients = _context.Ingredient.AsQueryable();

            IQueryable<Recipe> recipes = _context.Recipe.AsQueryable();
            recipes = recipes.Include(m => m.User).Include(m => m.Ingredients).Include(m => m.CommentsOnThisRecipe).ThenInclude(s => s.User);
            recipes = recipes.Include(m => m.CommentsOnThisRecipe).ThenInclude(m => m.Replies);
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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            IQueryable<Category> categories = _context.Category;
            IQueryable<Recipe> recipe = _context.Recipe;
            recipe = recipe.Include(m => m.Cuisine).Include(m => m.Complexity);
            var newrecipe = new AddRecipe
            {
                Categories = new SelectList(await categories.OrderBy(s => s.Tag).ToListAsync(), "CategoryId", "Tag")
            };
            return View(newrecipe);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(AddRecipe nrecipe, [FromForm] string[] ingredient)
        {
            if (ModelState.IsValid)
            {
                var url = UploadedFile(nrecipe);
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

                var recipe = new Recipe
                {
                    Title = nrecipe.Title,
                    Cuisine = nrecipe.CuisineFood,
                    Essay = nrecipe.Essay,
                    Preparation = nrecipe.Preparation,
                    PrepTime = nrecipe.PrepTime,
                    CookTime = nrecipe.CookTime,
                    Servings = nrecipe.Servings,
                    Complexity = nrecipe.Complex,
                    Upvotes = 0,
                    Downvotes = 0,
                    PostingDate = DateTime.UtcNow,
                    SpecialEquipment = nrecipe.SpecialEquipment,
                    UserPostedRecipeId = Useruserid,
                    Picture = "/Recipe Images/" + url
                };
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                foreach (int cid in nrecipe.SelectedCategories)
                {
                    RecipeInCategory cat = new RecipeInCategory
                    {
                        RecipeId = recipe.RecipeId,
                        CategoryId = cid
                    };
                    _context.Add(cat);
                    await _context.SaveChangesAsync();
                }

                foreach(var ingre in ingredient)
                {
                    Ingredient n = new Ingredient
                    {
                        IngredientUsed = ingre,
                        RecipeId = recipe.RecipeId
                    };
                    _context.Add(n);
                    await _context.SaveChangesAsync();
                }


                await _context.SaveChangesAsync();
                return RedirectToAction("ShowThisRecipe", "Recipes", new { id = recipe.RecipeId });
            }
            return View(nrecipe);
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
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IQueryable<Recipe> recipe = _context.Recipe.AsQueryable();
            recipe = recipe.Include(m => m.Ingredients).Include(m => m.CommentsOnThisRecipe).ThenInclude(m => m.Replies);
            var theone = recipe.FirstOrDefault(m => m.RecipeId == id);
            IQueryable<Comment> com = _context.Comment.AsQueryable();
            com = com.Where(m => m.CommentOnRecipeId == id);
            int rep = com.Count() - theone.CommentsOnThisRecipe.Count();
            int comments = theone.CommentsOnThisRecipe.Count();
            int replies= rep;
            int ingredients = theone.Ingredients.Count();
            
            while(replies>0)
            {
                var deletereply = _context.Comment.FirstOrDefault(m => m.CommentOnRecipeId == id);
                _context.Comment.Remove(deletereply);
                replies--;
                await _context.SaveChangesAsync();
            }
            while (comments > 0)
            {
                var deletecomments = _context.Comment.FirstOrDefault(m => m.CommentOnRecipeId == id);
                _context.Comment.Remove(deletecomments);
                comments--;
                await _context.SaveChangesAsync();
            }
            while (ingredients > 0)
            {
                var deleteingredient = _context.Ingredient.FirstOrDefault(m => m.RecipeId == id);
                _context.Ingredient.Remove(deleteingredient);
                ingredients--;
                await _context.SaveChangesAsync();
            }

            _context.Recipe.Remove(theone);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
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
            recipes = recipes.Where(m => m.RecipeId == id).Include(m => m.User).Include(m => m.Ingredients).Include(m => m.CommentsOnThisRecipe).ThenInclude(s => s.User);
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

        private string UploadedFile(AddRecipe model)
        {
            string uniqueFileName = null;

            if (model.UploadPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Recipe Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.UploadPicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.UploadPicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
