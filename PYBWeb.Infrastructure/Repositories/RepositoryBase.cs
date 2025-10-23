using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Repositories;

/// <summary>
/// Implementação base para repositórios
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected readonly PYBWebDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(PYBWebDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.Ativo);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => e.Ativo).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.DataCriacao = DateTime.Now;
        entity.Ativo = true;
        
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        entity.DataAtualizacao = DateTime.Now;
        
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;
            
        // Soft delete - marca como inativo
        entity.Ativo = false;
        entity.DataAtualizacao = DateTime.Now;
        
        await UpdateAsync(entity);
        return true;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id && e.Ativo);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync(e => e.Ativo);
    }
}