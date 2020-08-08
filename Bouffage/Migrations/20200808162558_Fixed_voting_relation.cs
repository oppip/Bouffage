using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouffage.Migrations
{
    public partial class Fixed_voting_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteComment_Comment_UserVotedThisComment",
                table: "VoteComment");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteRecipe_Recipe_UserVotedThisRecipe",
                table: "VoteRecipe");

            migrationBuilder.CreateIndex(
                name: "IX_VoteRecipe_RecipeGotVoted",
                table: "VoteRecipe",
                column: "RecipeGotVoted");

            migrationBuilder.CreateIndex(
                name: "IX_VoteComment_CommentGotVoted",
                table: "VoteComment",
                column: "CommentGotVoted");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteComment_Comment_CommentGotVoted",
                table: "VoteComment",
                column: "CommentGotVoted",
                principalTable: "Comment",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteRecipe_Recipe_RecipeGotVoted",
                table: "VoteRecipe",
                column: "RecipeGotVoted",
                principalTable: "Recipe",
                principalColumn: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoteComment_Comment_CommentGotVoted",
                table: "VoteComment");

            migrationBuilder.DropForeignKey(
                name: "FK_VoteRecipe_Recipe_RecipeGotVoted",
                table: "VoteRecipe");

            migrationBuilder.DropIndex(
                name: "IX_VoteRecipe_RecipeGotVoted",
                table: "VoteRecipe");

            migrationBuilder.DropIndex(
                name: "IX_VoteComment_CommentGotVoted",
                table: "VoteComment");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteComment_Comment_UserVotedThisComment",
                table: "VoteComment",
                column: "UserVotedThisComment",
                principalTable: "Comment",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoteRecipe_Recipe_UserVotedThisRecipe",
                table: "VoteRecipe",
                column: "UserVotedThisRecipe",
                principalTable: "Recipe",
                principalColumn: "RecipeId");
        }
    }
}
