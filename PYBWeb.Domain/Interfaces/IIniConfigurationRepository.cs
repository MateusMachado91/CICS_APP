namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para repositório de configurações INI
/// </summary>
public interface IIniConfigurationRepository : IRepositoryBase<Entities.IniConfiguration>
{
    /// <summary>
    /// Busca configurações por nome do arquivo
    /// </summary>
    Task<IEnumerable<Entities.IniConfiguration>> BuscarPorArquivoAsync(string nomeArquivo);
    
    /// <summary>
    /// Busca configuração específica por arquivo, seção e chave
    /// </summary>
    Task<Entities.IniConfiguration?> BuscarConfiguracaoAsync(string nomeArquivo, string secao, string chave);
    
    /// <summary>
    /// Busca todas as configurações de uma seção específica
    /// </summary>
    Task<IEnumerable<Entities.IniConfiguration>> BuscarPorSecaoAsync(string nomeArquivo, string secao);
    
    /// <summary>
    /// Atualiza o valor de uma configuração específica
    /// </summary>
    Task<bool> AtualizarValorConfiguracaoAsync(string nomeArquivo, string secao, string chave, string novoValor);
}