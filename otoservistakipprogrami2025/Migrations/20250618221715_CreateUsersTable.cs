using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace otoservistakipprogrami2025.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Kullanicilar",
                table: "Kullanicilar");

            migrationBuilder.RenameTable(
                name: "Kullanicilar",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "Sifre",
                table: "Users",
                newName: "Soyad");

            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "Users",
                newName: "KayitTarihi");

            migrationBuilder.RenameColumn(
                name: "KullaniciAdi",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Kullanicilar",
                newName: "Sifre");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Kullanicilar",
                newName: "Rol");

            migrationBuilder.RenameColumn(
                name: "KayitTarihi",
                table: "Kullanicilar",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Kullanicilar",
                newName: "KullaniciAdi");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Kullanicilar",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kullanicilar",
                table: "Kullanicilar",
                column: "Id");
        }
    }
}
