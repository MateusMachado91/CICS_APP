using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Repositories;

/// <summary>
/// Repositório para solicitações
/// </summary>
public class SolicitacaoRepository : RepositoryBase<Solicitacao>, ISolicitacaoRepository
{
    public SolicitacaoRepository(PYBWebDbContext context) : base(context)
    {
    }

    public override async Task<Solicitacao?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Include(s => s.Itens)
            .Include(s => s.Historico)
            .Include(s => s.Anexos)
            .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);
    }

    public override async Task<IEnumerable<Solicitacao>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetByStatusAsync(StatusSolicitacao status)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.Status == status && s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetByUsuarioSolicitanteAsync(string usuarioSolicitante)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.UsuarioSolicitante.ToLower() == usuarioSolicitante.ToLower() && s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetByTipoTabelaAsync(TipoTabela tipoTabela)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.TipoTabela == tipoTabela && s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetByAmbienteAsync(int ambienteId)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.AmbienteId == ambienteId && s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.DataCriacao >= dataInicio && s.DataCriacao <= dataFim && s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<Solicitacao>> GetProximasVencimentoAsync(int diasVencimento)
    {
        var dataLimite = DateTime.Now.AddDays(diasVencimento);
        
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => s.DataLimite.HasValue && 
                       s.DataLimite <= dataLimite && 
                       s.Status != StatusSolicitacao.Implementada &&
                       s.Status != StatusSolicitacao.Cancelada &&
                       s.Ativo)
            .OrderBy(s => s.DataLimite)
            .ToListAsync();
    }

    public async Task<string> GerarProximoNumeroAsync()
    {
        var ultimaSolicitacao = await _dbSet
            .Where(s => s.Numero.StartsWith(DateTime.Now.Year.ToString()))
            .OrderByDescending(s => s.Numero)
            .FirstOrDefaultAsync();

        if (ultimaSolicitacao == null)
        {
            return $"{DateTime.Now.Year}0001";
        }

        // Extrai o número sequencial do último número
        var ultimoNumero = ultimaSolicitacao.Numero.Substring(4);
        if (int.TryParse(ultimoNumero, out var numero))
        {
            return $"{DateTime.Now.Year}{(numero + 1):D4}";
        }

        return $"{DateTime.Now.Year}0001";
    }

    public async Task<IEnumerable<Solicitacao>> BuscarPorTextoAsync(string texto)
    {
        var textoLower = texto.ToLower();
        
        return await _dbSet
            .Include(s => s.Ambiente)
            .Where(s => (s.Titulo.ToLower().Contains(textoLower) ||
                        s.Descricao.ToLower().Contains(textoLower) ||
                        s.Numero.ToLower().Contains(textoLower) ||
                        s.UsuarioSolicitante.ToLower().Contains(textoLower)) &&
                       s.Ativo)
            .OrderByDescending(s => s.DataCriacao)
            .ToListAsync();
    }
}