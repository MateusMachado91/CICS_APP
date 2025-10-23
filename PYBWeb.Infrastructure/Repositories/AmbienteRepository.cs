using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Repositories;

/// <summary>
/// Repositório para ambientes
/// </summary>
public class AmbienteRepository : RepositoryBase<Ambiente>, IAmbienteRepository
{
    public AmbienteRepository(PYBWebDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ambiente>> GetByTipoAsync(TipoAmbiente tipo)
    {
        return await _dbSet
            .Where(a => a.Tipo == tipo && a.Ativo)
            .OrderBy(a => a.Nome)
            .ToListAsync();
    }

    public async Task<Ambiente?> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a => a.Nome.ToLower() == nome.ToLower() && a.Ativo);
    }

    public async Task<bool> ExisteNomeAsync(string nome, int? idExcluir = null)
    {
        var query = _dbSet.Where(a => a.Nome.ToLower() == nome.ToLower() && a.Ativo);
        
        if (idExcluir.HasValue)
        {
            query = query.Where(a => a.Id != idExcluir.Value);
        }
        
        return await query.AnyAsync();
    }
}