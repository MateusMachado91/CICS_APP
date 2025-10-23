using PYBWeb.Domain.Enums;

namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa uma solicitação de alteração em tabela CICS
/// </summary>
public class Solicitacao : BaseEntity
{
    /// <summary>
    /// Número sequencial da solicitação
    /// </summary>
    public string Numero { get; set; } = string.Empty;
    
    /// <summary>
    /// Título/resumo da solicitação
    /// </summary>
    public string Titulo { get; set; } = string.Empty;
    
    /// <summary>
    /// Descrição detalhada da solicitação
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Justificativa para a solicitação
    /// </summary>
    public string Justificativa { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo de tabela CICS afetada
    /// </summary>
    public TipoTabela TipoTabela { get; set; }
    
    /// <summary>
    /// Status atual da solicitação
    /// </summary>
    public StatusSolicitacao Status { get; set; }
    
    /// <summary>
    /// Data limite para implementação
    /// </summary>
    public DateTime? DataLimite { get; set; }
    
    /// <summary>
    /// Data de implementação efetiva
    /// </summary>
    public DateTime? DataImplementacao { get; set; }
    
    /// <summary>
    /// Prioridade da solicitação (1-5, sendo 1 mais alta)
    /// </summary>
    public int Prioridade { get; set; } = 3;
    
    /// <summary>
    /// Usuário solicitante
    /// </summary>
    public string UsuarioSolicitante { get; set; } = string.Empty;
    
    /// <summary>
    /// Área/departamento solicitante
    /// </summary>
    public string AreaSolicitante { get; set; } = string.Empty;
    
    /// <summary>
    /// Responsável pela implementação
    /// </summary>
    public string? ResponsavelImplementacao { get; set; }
    
    /// <summary>
    /// Observações gerais
    /// </summary>
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// ID do ambiente alvo
    /// </summary>
    public int AmbienteId { get; set; }
    
    /// <summary>
    /// Ambiente onde a solicitação será implementada
    /// </summary>
    public virtual Ambiente Ambiente { get; set; } = null!;
    
    /// <summary>
    /// Itens detalhados da solicitação
    /// </summary>
    public virtual ICollection<ItemSolicitacao> Itens { get; set; } = new List<ItemSolicitacao>();
    
    /// <summary>
    /// Histórico de alterações da solicitação
    /// </summary>
    public virtual ICollection<HistoricoSolicitacao> Historico { get; set; } = new List<HistoricoSolicitacao>();
    
    /// <summary>
    /// Anexos relacionados à solicitação
    /// </summary>
    public virtual ICollection<AnexoSolicitacao> Anexos { get; set; } = new List<AnexoSolicitacao>();
}