using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Repositories;

/// <summary>
/// Repositório para configurações INI
/// </summary>
public class IniConfigurationRepository : RepositoryBase<IniConfiguration>, IIniConfigurationRepository
{
    public IniConfigurationRepository(PYBWebDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Busca configurações por nome do arquivo
    /// </summary>
    public async Task<IEnumerable<IniConfiguration>> BuscarPorArquivoAsync(string nomeArquivo)
    {
        return await _context.IniConfigurations
            .Where(c => c.NomeArquivo == nomeArquivo && c.Ativo)
            .OrderBy(c => c.Secao)
            .ThenBy(c => c.Chave)
            .ToListAsync();
    }

    /// <summary>
    /// Busca configuração específica por arquivo, seção e chave
    /// </summary>
    public async Task<IniConfiguration?> BuscarConfiguracaoAsync(string nomeArquivo, string secao, string chave)
    {
        return await _context.IniConfigurations
            .FirstOrDefaultAsync(c => c.NomeArquivo == nomeArquivo && 
                                    c.Secao == secao && 
                                    c.Chave == chave && 
                                    c.Ativo);
    }

    /// <summary>
    /// Busca todas as configurações de uma seção específica
    /// </summary>
    public async Task<IEnumerable<IniConfiguration>> BuscarPorSecaoAsync(string nomeArquivo, string secao)
    {
        return await _context.IniConfigurations
            .Where(c => c.NomeArquivo == nomeArquivo && 
                       c.Secao == secao && 
                       c.Ativo)
            .OrderBy(c => c.Chave)
            .ToListAsync();
    }

    /// <summary>
    /// Atualiza o valor de uma configuração específica
    /// </summary>
    public async Task<bool> AtualizarValorConfiguracaoAsync(string nomeArquivo, string secao, string chave, string novoValor)
    {
        var configuracao = await BuscarConfiguracaoAsync(nomeArquivo, secao, chave);
        
        if (configuracao == null)
            return false;

        configuracao.Valor = novoValor;
        configuracao.DataAtualizacao = DateTime.Now;
        configuracao.UsuarioAtualizacao = "SYSTEM"; // TODO: Pegar do contexto de usuário

        await _context.SaveChangesAsync();
        return true;
    }
}