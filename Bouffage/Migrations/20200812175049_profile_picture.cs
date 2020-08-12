using Microsoft.EntityFrameworkCore.Migrations;

namespace Bouffage.Migrations
{
    public partial class profile_picture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "User");
        }
    }
}
