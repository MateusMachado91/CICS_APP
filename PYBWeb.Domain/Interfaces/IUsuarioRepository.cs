using PYBWeb.Domain.Entities;

namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para repositório de usuários
/// </summary>
public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    /// <summary>
    /// Obtém usuário por login
    /// </summary>
    /// <param name="login">Login do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    Task<Usuario?> GetByLoginAsync(string login);
    
    /// <summary>
    /// Obtém usuário por email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <returns>Usuário encontrado ou null</returns>
    Task<Usuario?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Obtém usuários por área
    /// </summary>
    /// <param name="area">Área/departamento</param>
    /// <returns>Lista de usuários</returns>
    Task<IEnumerable<Usuario>> GetByAreaAsync(string area);
    
    /// <summary>
    /// Obtém usuários administradores
    /// </summary>
    /// <returns>Lista de administradores</returns>
    Task<IEnumerable<Usuario>> GetAdministradoresAsync();
    
    /// <summary>
    /// Obtém usuários que podem aprovar solicitações
    /// </summary>
    /// <returns>Lista de aprovadores</returns>
    Task<IEnumerable<Usuario>> GetAprovadoresAsync();
    
    /// <summary>
    /// Verifica se existe usuário com o login
    /// </summary>
    /// <param name="login">Login do usuário</param>
    /// <param name="idExcluir">ID a ser excluído da verificação (para edição)</param>
    /// <returns>True se já existe</returns>
    Task<bool> ExisteLoginAsync(string login, int? idExcluir = null);
    
    /// <summary>
    /// Verifica se existe usuário com o email
    /// </summary>
    /// <param name="email">Email do usuário</param>
    /// <param name="idExcluir">ID a ser excluído da verificação (para edição)</param>
    /// <returns>True se já existe</returns>
    Task<bool> ExisteEmailAsync(string email, int? idExcluir = null);
}