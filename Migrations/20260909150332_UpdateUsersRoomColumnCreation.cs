using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMultidiciplinar.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsersRoomColumnCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatRooms",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRooms",
                table: "Users");
        }
    }
}
