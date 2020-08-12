using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bouffage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bouffage.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BouffageContext(serviceProvider.GetRequiredService<DbContextOptions<BouffageContext>>()))
            {
                if (context.User.Any() || context.Recipe.Any())
                {
                    return;
                }
                context.User.AddRange(
                    new User { /*UserId = 1,*/ Username = "Вилипче", DateCreated = DateTime.Parse("2017-8-1"), Email = "filip@edu.mk", Password = "$2a$11$jZbROFWgRgQB/3WM7bP5Nex5vaD5VVR.dXABr/lKhRWYjGF.TvFW2", Role = "Admin", Karma = 0, Following = 0, Followers = 0, Picture = "/Profile_pictures/frank.jpg", VerifiedEmail = true },
                    new User { /*UserId = 2,*/ Username = "Vladko", DateCreated = DateTime.Parse("2017-2-1"), Email = "vlatko@edu.mk", Password = "$2a$11$UwDPIFDOAjZ8QQAWbBvAyeYxaUw0IVU2I27XJeHJISeOvhtgm0V.6", Role = "User", Karma = 0, Following = 0, Followers = 0, Picture = "", VerifiedEmail = false },
                    new User { /*UserId = 3,*/ Username = "Петар", DateCreated = DateTime.Parse("1590-12-9"), Email = "pero@edu.mk", Password = "$2a$11$LJdaK7eLQFQyLViuHkBVF.e.PvHt9YwjmWFVaaT6DWOQ.qcrS/f7K", Role = "User", Karma = 0, Following = 0, Followers = 0, Picture = "", VerifiedEmail = false },
                    new User { /*UserId = 1,*/ Username = "IGOr4E", DateCreated = DateTime.Parse("2007-2-4"), Email = "igorche@edu.mk", Password = "$2a$11$LJdaK7eLQFQyLViuHkBVF.e.PvHt9YwjmWFVaaT6DWOQ.qcrS/f7K", Role = "User", Karma = 0, Following = 0, Followers = 0, Picture = "", VerifiedEmail = false },
                    new User { /*UserId = 1,*/ Username = "Доломир", DateCreated = DateTime.Parse("2017-2-5"), Email = "dolomire@edu.mk", Password = "$2a$11$LJdaK7eLQFQyLViuHkBVF.e.PvHt9YwjmWFVaaT6DWOQ.qcrS/f7K", Role = "User", Karma = 0, Following = 0, Followers = 0, Picture = "/Profile_pictures/frank.jpg", VerifiedEmail = false },
                    new User { /*UserId = 1,*/ Username = "Редбул", DateCreated = DateTime.Parse("2019-8-11"), Email = "redbull@edu.mk", Password = "$2a$11$LJdaK7eLQFQyLViuHkBVF.e.PvHt9YwjmWFVaaT6DWOQ.qcrS/f7K", Role = "Admin", Karma = 0, Following = 0, Followers = 0, Picture = "", VerifiedEmail = true },
                    new User { /*UserId = 1,*/ Username = "Фејт", DateCreated = DateTime.Parse("2000-1-1"), Email = "fate@edu.mk", Password = "$2a$11$LJdaK7eLQFQyLViuHkBVF.e.PvHt9YwjmWFVaaT6DWOQ.qcrS/f7K", Role = "User", Karma = 0, Following = 0, Followers = 0, Picture = "", VerifiedEmail = false }
                    );
                context.SaveChanges();

                context.Recipe.AddRange(
                    new Recipe
                    { 
                        /*RecipeId = 1,*/ 
                        Title = "Колачот на Ели",
                        Cuisine = Recipe.CuisineFood.Macedonian,
                        Essay = @"Фантастично благце, се гледа и по состојките, нели?
                                  Ставете се на уживање откако за многу брзо време ќе го направите.",
                        Preparation = @"Во сечко или процесор се мелат оревите, па се додава какаото и рогачот. Ги додаваме и останатите состојки и продолжуваме со мелење додека се направи смесата како тесто. Ова се постигнува само со добар процесор, со сечко ако правите ќе треба да го одморате, да не прегрее, а и тешко оди. Ама и јас немам процесор па се снаоѓам, нешто мелам во нутрибулет нешто во сечко, а мешам по некојпат и со лажица.
                                        Ја распоредуваме смесата во тава или модла.
                                        За кремот го топиме путерот на најниска температура и ги мешаме сите состојки. Мешањето го направив во нутрибулетот, можете да го направите и во стаклен сад со жица за матење. Ја прелевате основата со чоколадниот крем и го ставате да стои во фрижидер или може и во замрзнувач.
                                        Чашата е од 250 мл или 1 cup.",
                        PrepTime = 30, 
                        CookTime = 120, 
                        Servings = 12,
                        Complexity = Recipe.Complex.Easy,
                        Upvotes = 11, 
                        Downvotes = 2,
                        PostingDate = DateTime.Parse("2018-12-1"),
                        SpecialEquipment = "", 
                        UserPostedRecipeId = context.User.Single(d => d.Username == "Вилипче").UserId,
                        Picture = "/Recipe Images/kolac.jpg"
                    },
                    new Recipe
                    {
                        /*RecipeId = 2,*/
                        Title = "Шампињони",
                        Cuisine = Recipe.CuisineFood.Macedonian,
                        Essay = @"Еден пикантен вкус за љубителите на печурките.",
                        Preparation = @"За почеток кромидот исецкајте го по должина.
                                        Ставете го да се пржи на маслиново масло.
                                        Покријте со капак да си динста.Додајте го и ситно сецканото лукче.
                                        Кога се ќе смени боја,како стаклена, додајте ги печурките , ако се мали оставете ги цели,а ако се поголеми исецкајте ги на половинки.
                                        (Претходно исчистете ги убаво и исецкајте ги кај рачката )
                                        Промешајте и оставете да динста.
                                        Печурките ќе си пуштат како водичка па нека покрчкаат,повремено мешајќи ги..
                                        Зачинете ги со сол и црн бибер. Додајте го чили сосот.Јас ставив слатко лук сос што дополнителному даде убав вкус на јадењето.
                                        На крај полејте го со сок од лимон и магдонос.
                                        Промешајте и сервирајте.
                                        Ние го комбиниравме со узо,а вие одлучете сами )))))
                                        На здравје",
                        PrepTime = 30,
                        CookTime = 30,
                        Servings = 4,
                        Complexity = Recipe.Complex.Medium,
                        Upvotes = 11,
                        Downvotes = 2,
                        PostingDate = DateTime.Parse("2020-5-9"),
                        SpecialEquipment = "",
                        UserPostedRecipeId = context.User.Single(d => d.Username == "Доломир").UserId,
                        Picture = "/Recipe Images/Sampinjoni.jpg"
                    }
                    );

                context.SaveChanges();

                context.Comment.AddRange(
                    new Comment
                    {
                        /*CommentId = 1*/
                        Critique = @"Prekrasno i isto vremeno istotaka mnogu i fkusno e",
                        CommentPosted = DateTime.Parse("2020-5-10"),
                        Useful = 100, 
                        Useless = 13, 
                        TypeOfComments = Comment.TypeOfC.Comment,
                        UserCommentedId = context.User.Single(d => d.Username == "Вилипче").UserId,
                        CommentOnRecipeId = context.Recipe.Single(d => d.Title == "Колачот на Ели").RecipeId
                    },
                    new Comment
                    {
                        /*CommentId = 2*/
                        Critique = @"Bravo !",
                        CommentPosted = DateTime.Parse("2011-5-10"),
                        Useful = 0,
                        Useless = 4,
                        TypeOfComments = Comment.TypeOfC.Comment,
                        UserCommentedId = context.User.Single(d => d.Username == "Фејт").UserId,
                        CommentOnRecipeId = context.Recipe.Single(d => d.Title == "Шампињони").RecipeId
                    }
                    );
                context.SaveChanges();

                context.Ingredient.AddRange(
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "ШАМПИЊОНИ ",
                        RecipeId = context.Recipe.Single(d => d.Title == "Шампињони").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Чили ",
                        RecipeId = context.Recipe.Single(d => d.Title == "Шампињони").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Сол ",
                        RecipeId = context.Recipe.Single(d => d.Title == "Шампињони").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Бибер",
                        RecipeId = context.Recipe.Single(d => d.Title == "Шампињони").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Чоколадо",
                        RecipeId = context.Recipe.Single(d => d.Title == "Колачот на Ели").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Шечер",
                        RecipeId = context.Recipe.Single(d => d.Title == "Колачот на Ели").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Вода",
                        RecipeId = context.Recipe.Single(d => d.Title == "Колачот на Ели").RecipeId
                    },
                    new Ingredient
                    {
                        /*IngredientId = 1,*/
                        IngredientUsed = "Мљеко",
                        RecipeId = context.Recipe.Single(d => d.Title == "Колачот на Ели").RecipeId
                    }
                    ) ;
                context.SaveChanges();

                var i = 0;
                foreach (var c in Enum.GetNames(typeof(Category.Tags)))
                {
                    context.Category.AddRange(
                    new Category
                    {
                        Tag = (Category.Tags)i
                    }
                    );
                    i++;
                }
                context.SaveChanges();



                context.RecipeInCategory.AddRange(
                    new RecipeInCategory
                    {
                        /*RecipeInCategoryId = 1*/
                        RecipeId = context.Recipe.Single(p => p.Title == "Колачот на Ели").RecipeId,
                        CategoryId = context.Category.Single(p => p.Tag == Category.Tags.Vegetarian).CategoryId
                    },
                    new RecipeInCategory
                    {
                        /*RecipeInCategoryId = 2*/
                        RecipeId = context.Recipe.Single(p => p.Title == "Шампињони").RecipeId,
                        CategoryId = context.Category.Single(p => p.Tag == Category.Tags.Vegetarian).CategoryId
                    },
                    new RecipeInCategory
                    {
                        /*RecipeInCategoryId = 3*/
                        RecipeId = context.Recipe.Single(p => p.Title == "Шампињони").RecipeId,
                        CategoryId = context.Category.Single(p => p.Tag == Category.Tags.Vegan).CategoryId
                    }
                    );
                context.SaveChanges();
                
            }
        }
    }
}
