using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;

namespace PYBWeb.Domain.Interfaces;

/// <summary>
/// Interface para repositório de solicitações
/// </summary>
public interface ISolicitacaoRepository : IRepositoryBase<Solicitacao>
{
    /// <summary>
    /// Obtém solicitações por status
    /// </summary>
    /// <param name="status">Status da solicitação</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetByStatusAsync(StatusSolicitacao status);
    
    /// <summary>
    /// Obtém solicitações por usuário solicitante
    /// </summary>
    /// <param name="usuarioSolicitante">Login do usuário</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetByUsuarioSolicitanteAsync(string usuarioSolicitante);
    
    /// <summary>
    /// Obtém solicitações por tipo de tabela
    /// </summary>
    /// <param name="tipoTabela">Tipo da tabela CICS</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetByTipoTabelaAsync(TipoTabela tipoTabela);
    
    /// <summary>
    /// Obtém solicitações por ambiente
    /// </summary>
    /// <param name="ambienteId">ID do ambiente</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetByAmbienteAsync(int ambienteId);
    
    /// <summary>
    /// Obtém solicitações por período
    /// </summary>
    /// <param name="dataInicio">Data de início</param>
    /// <param name="dataFim">Data de fim</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    
    /// <summary>
    /// Obtém solicitações pendentes próximas do vencimento
    /// </summary>
    /// <param name="diasVencimento">Número de dias para vencimento</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> GetProximasVencimentoAsync(int diasVencimento);
    
    /// <summary>
    /// Gera próximo número de solicitação
    /// </summary>
    /// <returns>Próximo número disponível</returns>
    Task<string> GerarProximoNumeroAsync();
    
    /// <summary>
    /// Busca solicitações por texto livre
    /// </summary>
    /// <param name="texto">Texto a ser buscado</param>
    /// <returns>Lista de solicitações</returns>
    Task<IEnumerable<Solicitacao>> BuscarPorTextoAsync(string texto);
}