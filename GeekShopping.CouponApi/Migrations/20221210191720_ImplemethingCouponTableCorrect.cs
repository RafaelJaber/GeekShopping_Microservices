using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.CouponApi.Migrations
{
    public partial class ImplemethingCouponTableCorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_Coupon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiresDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserRegister = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_Coupon", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_Coupon");
        }
    }
}
