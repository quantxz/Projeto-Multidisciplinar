using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMultidiciplinar.Migrations
{
    /// <inheritdoc />
    public partial class updateAllTablesNameAsForegeinKeyRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_ProductId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_AuthorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AuthorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Photos_ProductId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersModelID",
                table: "Products",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsModelID",
                table: "Photos",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersModelID",
                table: "Messages",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UsersModelID",
                table: "Products",
                column: "UsersModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductsModelID",
                table: "Photos",
                column: "ProductsModelID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UsersModelID",
                table: "Messages",
                column: "UsersModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UsersModelID",
                table: "Messages",
                column: "UsersModelID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_ProductsModelID",
                table: "Photos",
                column: "ProductsModelID",
                principalTable: "Products",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_UsersModelID",
                table: "Products",
                column: "UsersModelID",
                principalTable: "Users",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UsersModelID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Products_ProductsModelID",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_UsersModelID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UsersModelID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Photos_ProductsModelID",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Messages_UsersModelID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UsersModelID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductsModelID",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UsersModelID",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Rating",
                keyValue: null,
                column: "Rating",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AuthorId",
                table: "Products",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Products_ProductId",
                table: "Photos",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_AuthorId",
                table: "Products",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
