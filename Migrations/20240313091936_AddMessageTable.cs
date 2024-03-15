using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAuth1.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userSendId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userReceiveId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Users_userReceiveId",
                        column: x => x.userReceiveId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_userSendId",
                        column: x => x.userSendId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_userReceiveId",
                table: "Messages",
                column: "userReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_userSendId",
                table: "Messages",
                column: "userSendId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
