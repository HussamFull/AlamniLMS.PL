using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlamniLMS.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCountColumnToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Enrollments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Enrollments");
        }
    }
}
