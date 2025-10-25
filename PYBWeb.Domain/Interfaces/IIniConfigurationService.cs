namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para serviço de configurações INI
/// </summary>
public interface IIniConfigurationService
{
    /// <summary>
    /// Carrega as configurações de um arquivo INI para o banco de dados
    /// </summary>
    Task<bool> CarregarArquivoIniAsync(string caminhoArquivo);
    
    /// <summary>
    /// Obtém o valor de uma configuração específica
    /// </summary>
    Task<string?> ObterValorConfiguracaoAsync(string nomeArquivo, string secao, string chave);
    
    /// <summary>
    /// Obtém todas as configurações de um arquivo
    /// </summary>
    Task<Dictionary<string, Dictionary<string, string>>> ObterTodasConfiguracoesAsync(string nomeArquivo);
    
    /// <summary>
    /// Atualiza uma configuração específica
    /// </summary>
    Task<bool> AtualizarConfiguracaoAsync(string nomeArquivo, string secao, string chave, string novoValor);
    
    /// <summary>
    /// Exporta as configurações atuais para um arquivo INI
    /// </summary>
    Task<bool> ExportarParaArquivoIniAsync(string nomeArquivo, string caminhoDestino);
    
    /// <summary>
    /// Sincroniza todas as configurações dos arquivos INI da pasta DATA
    /// </summary>
    Task<bool> SincronizarConfiguracoesAsync();
}