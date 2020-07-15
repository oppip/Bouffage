using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouffage.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(nullable: false),
                    Tag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 35, nullable: false),
                    Password = table.Column<string>(maxLength: 60, nullable: true),
                    Username = table.Column<string>(maxLength: 40, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    Role = table.Column<string>(maxLength: 20, nullable: true),
                    Karma = table.Column<int>(nullable: false),
                    Following = table.Column<int>(nullable: false),
                    Followers = table.Column<int>(nullable: false),
                    VerifiedEmail = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Following",
                columns: table => new
                {
                    FollowingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFollowingId = table.Column<int>(nullable: false),
                    UserFolloweeId = table.Column<int>(nullable: false),
                    DateFollowed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Following", x => x.FollowingId);
                    table.ForeignKey(
                        name: "FK_Following_User_UserFolloweeId",
                        column: x => x.UserFolloweeId,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Following_User_UserFollowingId",
                        column: x => x.UserFollowingId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Recipe",
                columns: table => new
                {
                    RecipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    CuisineId = table.Column<int>(nullable: false),
                    Cuisine = table.Column<int>(nullable: false),
                    Essay = table.Column<string>(maxLength: 700, nullable: true),
                    Preparation = table.Column<string>(nullable: true),
                    PrepTime = table.Column<int>(nullable: false),
                    CookTime = table.Column<int>(nullable: false),
                    Servings = table.Column<int>(nullable: false),
                    ComplexityId = table.Column<int>(nullable: false),
                    Complexity = table.Column<int>(nullable: false),
                    Upvotes = table.Column<int>(nullable: false),
                    Downvotes = table.Column<int>(nullable: false),
                    PostingDate = table.Column<DateTime>(nullable: true),
                    SpecialEquipment = table.Column<string>(maxLength: 100, nullable: true),
                    UserPostedRecipeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipe", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipe_User_UserPostedRecipeId",
                        column: x => x.UserPostedRecipeId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Critique = table.Column<string>(maxLength: 500, nullable: false),
                    CommentPosted = table.Column<DateTime>(nullable: true),
                    Useful = table.Column<int>(nullable: false),
                    Useless = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    TypeOfComments = table.Column<int>(nullable: false),
                    UserCommentedId = table.Column<int>(nullable: true),
                    CommentOnRecipeId = table.Column<int>(nullable: true),
                    ReplyCommentId = table.Column<int>(nullable: true),
                    RecipeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Recipe_CommentOnRecipeId",
                        column: x => x.CommentOnRecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ReplyCommentId",
                        column: x => x.ReplyCommentId,
                        principalTable: "Comment",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserCommentedId",
                        column: x => x.UserCommentedId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientUsed = table.Column<string>(maxLength: 70, nullable: false),
                    RecipeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeInCategory",
                columns: table => new
                {
                    RecipeInCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInCategory", x => x.RecipeInCategoryId);
                    table.ForeignKey(
                        name: "FK_RecipeInCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeInCategory_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoteRecipe",
                columns: table => new
                {
                    VoteRecipeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserVotedThisRecipe = table.Column<int>(nullable: false),
                    RecipeGotVoted = table.Column<int>(nullable: false),
                    UpOrDown = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteRecipe", x => x.VoteRecipeId);
                    table.ForeignKey(
                        name: "FK_VoteRecipe_Recipe_UserVotedThisRecipe",
                        column: x => x.UserVotedThisRecipe,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId");
                    table.ForeignKey(
                        name: "FK_VoteRecipe_User_UserVotedThisRecipe",
                        column: x => x.UserVotedThisRecipe,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "VoteComment",
                columns: table => new
                {
                    VoteCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserVotedThisComment = table.Column<int>(nullable: false),
                    CommentGotVoted = table.Column<int>(nullable: false),
                    UpOrDown = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteComment", x => x.VoteCommentId);
                    table.ForeignKey(
                        name: "FK_VoteComment_Comment_UserVotedThisComment",
                        column: x => x.UserVotedThisComment,
                        principalTable: "Comment",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_VoteComment_User_UserVotedThisComment",
                        column: x => x.UserVotedThisComment,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentOnRecipeId",
                table: "Comment",
                column: "CommentOnRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RecipeId",
                table: "Comment",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReplyCommentId",
                table: "Comment",
                column: "ReplyCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserCommentedId",
                table: "Comment",
                column: "UserCommentedId");

            migrationBuilder.CreateIndex(
                name: "IX_Following_UserFolloweeId",
                table: "Following",
                column: "UserFolloweeId");

            migrationBuilder.CreateIndex(
                name: "IX_Following_UserFollowingId",
                table: "Following",
                column: "UserFollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeId",
                table: "Ingredient",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_UserPostedRecipeId",
                table: "Recipe",
                column: "UserPostedRecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInCategory_CategoryId",
                table: "RecipeInCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInCategory_RecipeId",
                table: "RecipeInCategory",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteComment_UserVotedThisComment",
                table: "VoteComment",
                column: "UserVotedThisComment");

            migrationBuilder.CreateIndex(
                name: "IX_VoteRecipe_UserVotedThisRecipe",
                table: "VoteRecipe",
                column: "UserVotedThisRecipe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Following");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "RecipeInCategory");

            migrationBuilder.DropTable(
                name: "VoteComment");

            migrationBuilder.DropTable(
                name: "VoteRecipe");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Recipe");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
