using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;

namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para repositório de ambientes
/// </summary>
public interface IAmbienteRepository : IRepositoryBase<Ambiente>
{
    /// <summary>
    /// Obtém ambientes por tipo
    /// </summary>
    /// <param name="tipo">Tipo do ambiente</param>
    /// <returns>Lista de ambientes</returns>
    Task<IEnumerable<Ambiente>> GetByTipoAsync(TipoAmbiente tipo);
    
    /// <summary>
    /// Obtém ambiente por nome
    /// </summary>
    /// <param name="nome">Nome do ambiente</param>
    /// <returns>Ambiente encontrado ou null</returns>
    Task<Ambiente?> GetByNomeAsync(string nome);
    
    /// <summary>
    /// Verifica se existe ambiente com o nome
    /// </summary>
    /// <param name="nome">Nome do ambiente</param>
    /// <param name="idExcluir">ID a ser excluído da verificação (para edição)</param>
    /// <returns>True se já existe</returns>
    Task<bool> ExisteNomeAsync(string nome, int? idExcluir = null);
}