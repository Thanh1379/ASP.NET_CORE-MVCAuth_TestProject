using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAuth1.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendList",
                columns: table => new
                {
                    FriendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user1Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    user2Id = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendList", x => x.FriendId);
                    table.ForeignKey(
                        name: "FK_FriendList_Users_user1Id",
                        column: x => x.user1Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendList_Users_user2Id",
                        column: x => x.user2Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_user1Id",
                table: "FriendList",
                column: "user1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_user2Id",
                table: "FriendList",
                column: "user2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendList");
        }
    }
}
