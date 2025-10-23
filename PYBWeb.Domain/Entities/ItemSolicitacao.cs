namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa um item específico dentro de uma solicitação CICS
/// </summary>
public class ItemSolicitacao : BaseEntity
{
    /// <summary>
    /// Nome da entrada/recurso na tabela CICS
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Ação a ser realizada (Incluir, Alterar, Excluir)
    /// </summary>
    public string Acao { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor atual (para alterações)
    /// </summary>
    public string? ValorAtual { get; set; }
    
    /// <summary>
    /// Novo valor proposto
    /// </summary>
    public string? NovoValor { get; set; }
    
    /// <summary>
    /// Configurações específicas da tabela CICS em formato JSON
    /// </summary>
    public string ConfiguracaoCics { get; set; } = string.Empty;
    
    /// <summary>
    /// Comentários específicos do item
    /// </summary>
    public string? Comentarios { get; set; }
    
    /// <summary>
    /// Ordem de execução do item
    /// </summary>
    public int Ordem { get; set; }
    
    /// <summary>
    /// ID da solicitação pai
    /// </summary>
    public int SolicitacaoId { get; set; }
    
    /// <summary>
    /// Solicitação pai
    /// </summary>
    public virtual Solicitacao Solicitacao { get; set; } = null!;
}