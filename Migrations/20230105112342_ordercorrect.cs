using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producer.Migrations
{
    public partial class ordercorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_AspNetUsers_IdentityUserId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_GiftItems_GiftItemId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_GiftItems_GiftItemId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IdentityUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_GiftItemId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_GiftItemId",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_IdentityUserId",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "GiftItemId",
                table: "CartDetails",
                newName: "GiftItemID");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartDetails");

            migrationBuilder.RenameColumn(
                name: "GiftItemID",
                table: "CartDetails",
                newName: "GiftItemId");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "CartDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdentityUserId",
                table: "Orders",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_GiftItemId",
                table: "OrderDetails",
                column: "GiftItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_GiftItemId",
                table: "CartDetails",
                column: "GiftItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_IdentityUserId",
                table: "CartDetails",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_AspNetUsers_IdentityUserId",
                table: "CartDetails",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_GiftItems_GiftItemId",
                table: "CartDetails",
                column: "GiftItemId",
                principalTable: "GiftItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_GiftItems_GiftItemId",
                table: "OrderDetails",
                column: "GiftItemId",
                principalTable: "GiftItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_IdentityUserId",
                table: "Orders",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
