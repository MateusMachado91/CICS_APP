using PYBWeb.Domain.Enums;

namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa o histórico de alterações de uma solicitação
/// </summary>
public class HistoricoSolicitacao : BaseEntity
{
    /// <summary>
    /// Status anterior da solicitação
    /// </summary>
    public StatusSolicitacao? StatusAnterior { get; set; }
    
    /// <summary>
    /// Novo status da solicitação
    /// </summary>
    public StatusSolicitacao StatusNovo { get; set; }
    
    /// <summary>
    /// Motivo da alteração
    /// </summary>
    public string Motivo { get; set; } = string.Empty;
    
    /// <summary>
    /// Observações da alteração
    /// </summary>
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// Usuário responsável pela alteração
    /// </summary>
    public string UsuarioAlteracao { get; set; } = string.Empty;
    
    /// <summary>
    /// Data da alteração
    /// </summary>
    public DateTime DataAlteracao { get; set; }
    
    /// <summary>
    /// ID da solicitação
    /// </summary>
    public int SolicitacaoId { get; set; }
    
    /// <summary>
    /// Solicitação relacionada
    /// </summary>
    public virtual Solicitacao Solicitacao { get; set; } = null!;
}