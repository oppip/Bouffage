using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bouffage.Models;

namespace Bouffage.Data
{
    public class BouffageContext : DbContext
    {
        public BouffageContext (DbContextOptions<BouffageContext> options)
            : base(options)
        {
        }

        public DbSet<Bouffage.Models.Comment> Comment { get; set; }

        public DbSet<Bouffage.Models.Recipe> Recipe { get; set; }

        public DbSet<Bouffage.Models.User> User { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            {
                /* base.OnModelCreating(builder);

                 builder.Entity<Following>()
                     .HasKey(c => new { c.UserFolloweeId , c.UserFollowingId });

                 builder.Entity<RecipeInCategory>()
                     .HasKey(c => new { c.RecipeId, c.CategoryId });

                 builder.Entity<VoteComment>()
                     .HasKey(c => new { c.CommentGotVoted , c.UserVotedThisComment });

                 builder.Entity<VoteRecipe>()
                     .HasKey(c => new { c.RecipeGotVoted, c.UserVotedThisRecipe });*/
            }

            { //Following junction table
                builder.Entity<Following>()
                    .HasOne<User>(p => p.UserFollowing)
                    .WithMany(p => p.FollowMe)
                    .HasForeignKey(p => p.UserFollowingId).OnDelete(DeleteBehavior.NoAction);

                builder.Entity<Following>()
                    .HasOne<User>(p => p.UserFollowee)
                    .WithMany(p => p.IFollow)
                    .HasForeignKey(p => p.UserFolloweeId).OnDelete(DeleteBehavior.NoAction);
            }

            { //Junction table for recipes in category
                builder.Entity<RecipeInCategory>()
                    .HasOne<Category>(p => p.Category)
                    .WithMany(p => p.RecipesInThisCategory)
                    .HasForeignKey(p => p.CategoryId);

                builder.Entity<RecipeInCategory>()
                    .HasOne<Recipe>(p => p.Recipe)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(p => p.RecipeId);
            }

            { //junction table for comment
                builder.Entity<Comment>()
                    .HasOne<Recipe>(p => p.Recipe)
                    .WithMany(p => p.CommentsOnThisRecipe)
                    .HasForeignKey(p => p.CommentOnRecipeId);

                builder.Entity<Comment>()
                    .HasOne<User>(p => p.User)
                    .WithMany(p => p.MyComments)
                    .HasForeignKey(p => p.UserCommentedId);

                builder.Entity<Comment>()
                    .HasOne<Comment>(p => p.Reply)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(p => p.ReplyCommentId);
            }

            { //junction table for recipe
                builder.Entity<Recipe>()
                    .HasOne<User>(p => p.User)
                    .WithMany(p => p.MyRecipes)
                    .HasForeignKey(p => p.UserPostedRecipeId);
            }

            { //junction table for voting recipes
                builder.Entity<VoteRecipe>()
                    .HasOne<User>(p => p.User)
                    .WithMany(p => p.VotedRecipes)
                    .HasForeignKey(p => p.UserVotedThisRecipe).OnDelete(DeleteBehavior.NoAction);
                builder.Entity<VoteRecipe>()
                   .HasOne<Recipe>(p => p.Recipe)
                   .WithMany(p => p.UsersThatVotedRecipe)
                   .HasForeignKey(p => p.UserVotedThisRecipe).OnDelete(DeleteBehavior.NoAction);
            }

            { //junction table for voting comments
                builder.Entity<VoteComment>()
                    .HasOne<User>(p => p.User)
                    .WithMany(p => p.VotedComments)
                    .HasForeignKey(p => p.UserVotedThisComment).OnDelete(DeleteBehavior.NoAction);
                builder.Entity<VoteComment>()
                   .HasOne<Comment>(p => p.Comment)
                   .WithMany(p => p.UsersThatVotedComment)
                   .HasForeignKey(p => p.UserVotedThisComment).OnDelete(DeleteBehavior.NoAction);
            }
        }



        public DbSet<Bouffage.Models.Category> Category { get; set; }


        public DbSet<Bouffage.Models.Ingredient> Ingredient { get; set; }


        public DbSet<Bouffage.Models.Following> Following { get; set; }


        public DbSet<Bouffage.Models.RecipeInCategory> RecipeInCategory { get; set; }


        public DbSet<Bouffage.Models.VoteRecipe> VoteRecipe { get; set; }


        public DbSet<Bouffage.Models.VoteComment> VoteComment { get; set; }



    }
}
