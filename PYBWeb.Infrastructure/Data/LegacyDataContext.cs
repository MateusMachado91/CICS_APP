using Microsoft.EntityFrameworkCore;

namespace PYBWeb.Infrastructure.Data;

/// <summary>
/// Contexto para acessar os bancos SQLite legados na pasta DATA
/// </summary>
public class LegacyDataContext : DbContext
{
    private readonly string _connectionString;

    public LegacyDataContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Como são bancos legados, vamos usar configurações mais flexíveis
        // para mapear as tabelas existentes sem modificá-las
        
        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Executa uma consulta SQL diretamente no banco SQLite
    /// </summary>
    public async Task<List<Dictionary<string, object>>> ExecutarConsultaAsync(string sql)
    {
        var resultado = new List<Dictionary<string, object>>();
        
        using var connection = Database.GetDbConnection();
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var linha = new Dictionary<string, object>();
            
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var nomeColuna = reader.GetName(i);
                var valor = reader.IsDBNull(i) ? null : reader.GetValue(i);
                linha[nomeColuna] = valor;
            }
            
            resultado.Add(linha);
        }
        
        return resultado;
    }

    /// <summary>
    /// Obtém informações sobre as tabelas existentes no banco
    /// </summary>
    public async Task<List<string>> ObterNomesTabelasAsync()
    {
        var tabelas = await ExecutarConsultaAsync(
            "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'");
        
        return tabelas.Select(t => t["name"].ToString()).Where(n => !string.IsNullOrEmpty(n)).Cast<string>().ToList();
    }

    /// <summary>
    /// Obtém informações sobre as colunas de uma tabela
    /// </summary>
    public async Task<List<Dictionary<string, object>>> ObterEstruturaTabelaAsync(string nomeTabela)
    {
        return await ExecutarConsultaAsync($"PRAGMA table_info({nomeTabela})");
    }
}