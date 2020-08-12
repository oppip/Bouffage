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
using Microsoft.AspNetCore.Authorization;
using BC = BCrypt.Net.BCrypt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Bouffage.Controllers
{
    public class UsersController : Controller
    {
        private readonly BouffageContext _context;
        private readonly IHostingEnvironment webHostEnvironment;

        public UsersController(BouffageContext context, IHostingEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Email,Password,Username,DateCreated,Role,Karma,Following,Followers,VerifiedEmail")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,Username,DateCreated,Role,Karma,Following,Followers,VerifiedEmail")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
                
        public async Task<IActionResult> GetThisGuysProfile(int? id)
        {
            if (id == null)
            { 
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == id);
            IQueryable<Ingredient> ingredients = _context.Ingredient.AsQueryable();
            IQueryable<Recipe> recipes = _context.Recipe.AsQueryable();
            IQueryable<Comment> comments = _context.Comment.AsQueryable();

            recipes = recipes.Where(p => p.UserPostedRecipeId == user.UserId);
            comments = comments.Where(p => p.UserCommentedId == user.UserId).Include(p => p.Recipe);

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
            var following = await _context.Following.FirstOrDefaultAsync(m => m.UserFolloweeId == id && m.UserFollowingId == Useruserid);
            if (following == null)
            {
                ViewBag.followingeachother = false;
            }
            else
            {
                ViewBag.followingeachother = true;
            }

            UserRecipesViewModel userRecipesVM = new UserRecipesViewModel
            {
                Recipes = await recipes.ToListAsync(),
                Comments = await comments.ToListAsync(),
                Ingredients = await ingredients.ToListAsync(),
                User = user
            };


            if (user == null)
            {
                return NotFound();
            }
            return View(userRecipesVM);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }


        public async Task<IActionResult> Authenticate([FromForm] string Email, [FromForm] string Password)
        {
            var _users = await _context.User.ToListAsync();
            var HashedPassword = BC.HashPassword(Password);
            var user = _users.SingleOrDefault(x => x.Email == Email && BC.Verify(Password, x.Password));
            string key = "MyCookie";
            CookieOptions cookieOptions = new CookieOptions();
            //cookieOptions.Expires = DateTime.Now.AddMinutes(2);

            if (user == null)
            {
                var error = "Неточна е-пошта или лозинка&%&" + Email ;
                ViewBag.email = Email;
                ViewData["show"] = true;
                Response.Cookies.Append("error", error, cookieOptions);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                string value = user.UserId.ToString() + "&%&" + user.Username + "&%&" + user.Role;
                Response.Cookies.Append(key, value, cookieOptions);

                var claims = new List<Claim>
               {
                   new Claim(ClaimTypes.Name, Email)
               };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            Response.Cookies.Delete("MyCookie");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignUp()
        {
            /*var str = "document.getElementById('id01').style.display='block'";
            return Content(str);*//*
            ViewData["show"] = true;        //is there a function returntourl for signing up too
            return View("Index");*/
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel user)
        {
            if (ModelState.IsValid)
            {
                string url = UploadedFile(user);
                var flag = false;
                var uniqueness = _context.User.Any(x => x.Email == user.Email);
                if (user.Password != user.ConfirmedPassword)
                {
                    flag = true;
                    ViewBag.password = "Внесените лозинки не се совпаѓаат";
                    ViewBag.epassword = "*";
                }
                if (uniqueness)
                {
                    flag = true;
                    ViewBag.email = "Внесената е-пошта е веќе искористена";
                    ViewBag.eemail = "*";
                }
                if (flag)
                {
                    return View(user);
                }
                else
                {
                    User newuser = new User
                    {
                        Email = user.Email,
                        Password = BC.HashPassword(user.Password),
                        Followers = 0,
                        Following = 0,
                        Karma = 0,
                        DateCreated = DateTime.UtcNow,
                        Username = user.Username,
                        Role = "User",
                        Picture = "/Profile_pictures/"+ url
                    };
                    _context.Add(newuser);
                    await _context.SaveChangesAsync();

                    //automatically log in
                    string key = "MyCookie";
                    string value = newuser.UserId.ToString() + "&%&" + user.Username + "&%&" + user.Role;
                    CookieOptions cookieOptions = new CookieOptions();
                    //cookieOptions.Expires = DateTime.Now.AddMinutes(2);
                    Response.Cookies.Append(key, value, cookieOptions);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email)
                    };
                    var identity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View(user);
            }
        }

        public async Task<IActionResult> MakeUserAdmin(int id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            user.Role = "Admin";
            try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveAdminStatus(int id)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            user.Role = "User";
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> DeleteAccountA(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            Response.Cookies.Delete("MyCookie");
            await HttpContext.SignOutAsync();
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private string UploadedFile(SignUpViewModel model)
        {
            string uniqueFileName = null;

            if (model.UploadPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Profile_pictures");
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
