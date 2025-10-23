using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PYBWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ambientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Servidor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Porta = table.Column<int>(type: "int", nullable: true),
                    RegiaoCics = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsAdministrador = table.Column<bool>(type: "bit", nullable: false),
                    PodeAprovar = table.Column<bool>(type: "bit", nullable: false),
                    UltimoLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Justificativa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TipoTabela = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataLimite = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataImplementacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Prioridade = table.Column<int>(type: "int", nullable: false),
                    UsuarioSolicitante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaSolicitante = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResponsavelImplementacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AmbienteId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Ambientes_AmbienteId",
                        column: x => x.AmbienteId,
                        principalTable: "Ambientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnexosSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TipoMime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SolicitacaoId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnexosSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnexosSolicitacao_Solicitacoes_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoSolicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusAnterior = table.Column<int>(type: "int", nullable: true),
                    StatusNovo = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UsuarioAlteracao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolicitacaoId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoSolicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoSolicitacoes_Solicitacoes_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensSolicitacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Acao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ValorAtual = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NovoValor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConfiguracaoCics = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    SolicitacaoId = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCriacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioAtualizacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensSolicitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensSolicitacao_Solicitacoes_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "Solicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ambientes",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "Descricao", "Nome", "Porta", "RegiaoCics", "Servidor", "Tipo", "UsuarioAtualizacao", "UsuarioCriacao" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 10, 20, 18, 55, 54, 12, DateTimeKind.Local).AddTicks(6825), "Ambiente de desenvolvimento CICS", "Desenvolvimento", 1433, "CICSDV01", "CICS-DEV", 1, null, "SYSTEM" },
                    { 2, true, null, new DateTime(2025, 10, 20, 18, 55, 54, 15, DateTimeKind.Local).AddTicks(6585), "Ambiente de teste CICS", "Teste", 1433, "CICSTS01", "CICS-TST", 2, null, "SYSTEM" },
                    { 3, true, null, new DateTime(2025, 10, 20, 18, 55, 54, 15, DateTimeKind.Local).AddTicks(6613), "Ambiente de homologação CICS", "Homologação", 1433, "CICSHM01", "CICS-HML", 3, null, "SYSTEM" },
                    { 4, true, null, new DateTime(2025, 10, 20, 18, 55, 54, 15, DateTimeKind.Local).AddTicks(6615), "Ambiente de produção CICS", "Produção", 1433, "CICSPR01", "CICS-PRD", 4, null, "SYSTEM" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Area", "Ativo", "Cargo", "DataAtualizacao", "DataCriacao", "Email", "IsAdministrador", "Login", "Nome", "PodeAprovar", "Telefone", "UltimoLogin", "UsuarioAtualizacao", "UsuarioCriacao" },
                values: new object[] { 1, "TI", true, "Administrador", null, new DateTime(2025, 10, 20, 18, 55, 54, 16, DateTimeKind.Local).AddTicks(4635), "admin@banrisul.com.br", true, "admin", "Administrador do Sistema", true, null, null, null, "SYSTEM" });

            migrationBuilder.CreateIndex(
                name: "IX_Ambientes_Nome",
                table: "Ambientes",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnexosSolicitacao_SolicitacaoId",
                table: "AnexosSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoSolicitacoes_SolicitacaoId",
                table: "HistoricoSolicitacoes",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensSolicitacao_SolicitacaoId",
                table: "ItensSolicitacao",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_AmbienteId",
                table: "Solicitacoes",
                column: "AmbienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_Numero",
                table: "Solicitacoes",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_UsuarioId",
                table: "Solicitacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Login",
                table: "Usuarios",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnexosSolicitacao");

            migrationBuilder.DropTable(
                name: "HistoricoSolicitacoes");

            migrationBuilder.DropTable(
                name: "ItensSolicitacao");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Ambientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
