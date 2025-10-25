using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PYBWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIniConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IniConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Secao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Chave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TipoConfiguracao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCritica = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IniConfigurations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IniConfigurations_NomeArquivo_Secao_Chave",
                table: "IniConfigurations",
                columns: new[] { "NomeArquivo", "Secao", "Chave" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IniConfigurations");
        }
    }
}