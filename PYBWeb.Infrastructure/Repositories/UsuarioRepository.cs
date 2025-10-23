using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Repositories;

/// <summary>
/// Repositório para usuários
/// </summary>
public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(PYBWebDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByLoginAsync(string login)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower() && u.Ativo);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
    }

    public async Task<IEnumerable<Usuario>> GetByAreaAsync(string area)
    {
        return await _dbSet
            .Where(u => u.Area.ToLower() == area.ToLower() && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetAdministradoresAsync()
    {
        return await _dbSet
            .Where(u => u.IsAdministrador && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetAprovadoresAsync()
    {
        return await _dbSet
            .Where(u => u.PodeAprovar && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<bool> ExisteLoginAsync(string login, int? idExcluir = null)
    {
        var query = _dbSet.Where(u => u.Login.ToLower() == login.ToLower() && u.Ativo);
        
        if (idExcluir.HasValue)
        {
            query = query.Where(u => u.Id != idExcluir.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task<bool> ExisteEmailAsync(string email, int? idExcluir = null)
    {
        var query = _dbSet.Where(u => u.Email.ToLower() == email.ToLower() && u.Ativo);
        
        if (idExcluir.HasValue)
        {
            query = query.Where(u => u.Id != idExcluir.Value);
        }
        
        return await query.AnyAsync();
    }
}