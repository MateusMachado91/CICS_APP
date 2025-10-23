using PYBWeb.Domain.Entities;

namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface base para repositórios
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
public interface IRepositoryBase<T> where T : BaseEntity
{
    /// <summary>
    /// Obtém uma entidade por ID
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <returns>Entidade encontrada ou null</returns>
    Task<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Obtém todas as entidades
    /// </summary>
    /// <returns>Lista de entidades</returns>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// Obtém entidades com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página</param>
    /// <param name="pageSize">Tamanho da página</param>
    /// <returns>Lista paginada de entidades</returns>
    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
    
    /// <summary>
    /// Adiciona uma nova entidade
    /// </summary>
    /// <param name="entity">Entidade a ser adicionada</param>
    /// <returns>Entidade adicionada</returns>
    Task<T> AddAsync(T entity);
    
    /// <summary>
    /// Atualiza uma entidade existente
    /// </summary>
    /// <param name="entity">Entidade a ser atualizada</param>
    /// <returns>Entidade atualizada</returns>
    Task<T> UpdateAsync(T entity);
    
    /// <summary>
    /// Remove uma entidade por ID
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <returns>True se removida com sucesso</returns>
    Task<bool> DeleteAsync(int id);
    
    /// <summary>
    /// Verifica se uma entidade existe
    /// </summary>
    /// <param name="id">ID da entidade</param>
    /// <returns>True se existe</returns>
    Task<bool> ExistsAsync(int id);
    
    /// <summary>
    /// Conta o total de entidades
    /// </summary>
    /// <returns>Número total de entidades</returns>
    Task<int> CountAsync();
}