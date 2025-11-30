using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fashion_sales.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailedAddressFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistrictCode",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ProvinceCode",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "WardCode",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DistrictCode",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ProvinceCode",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "WardCode",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DistrictCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "AspNetUsers");
        }
    }
}
