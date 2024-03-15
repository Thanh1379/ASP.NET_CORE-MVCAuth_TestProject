using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCAuth1.Migrations
{
    /// <inheritdoc />
    public partial class AddSendingTimeToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SendingTime",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendingTime",
                table: "Messages");
        }
    }
}
