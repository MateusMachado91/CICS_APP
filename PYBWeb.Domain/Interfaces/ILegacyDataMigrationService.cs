namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para serviço de migração de dados legados
/// </summary>
public interface ILegacyDataMigrationService
{
    /// <summary>
    /// Analisa a estrutura dos bancos SQLite legados
    /// </summary>
    Task<Dictionary<string, object>> AnalisarEstruturaBancosAsync();
    
    /// <summary>
    /// Importa dados dos usuários/colaboradores do banco legado
    /// </summary>
    Task<bool> ImportarUsuariosAsync();
    
    /// <summary>
    /// Importa dados das solicitações do banco legado
    /// </summary>
    Task<bool> ImportarSolicitacoesAsync();
    
    /// <summary>
    /// Executa migração completa dos dados legados
    /// </summary>
    Task<bool> ExecutarMigracaoCompletaAsync();
    
    /// <summary>
    /// Valida a integridade dos dados migrados
    /// </summary>
    Task<Dictionary<string, object>> ValidarDadosMigradosAsync();
}