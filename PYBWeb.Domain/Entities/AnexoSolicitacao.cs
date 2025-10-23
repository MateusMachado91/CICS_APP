namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa um anexo associado a uma solicitação
/// </summary>
public class AnexoSolicitacao : BaseEntity
{
    /// <summary>
    /// Nome original do arquivo
    /// </summary>
    public string NomeArquivo { get; set; } = string.Empty;
    
    /// <summary>
    /// Caminho onde o arquivo está armazenado
    /// </summary>
    public string CaminhoArquivo { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo MIME do arquivo
    /// </summary>
    public string TipoMime { get; set; } = string.Empty;
    
    /// <summary>
    /// Tamanho do arquivo em bytes
    /// </summary>
    public long TamanhoBytes { get; set; }
    
    /// <summary>
    /// Descrição do anexo
    /// </summary>
    public string? Descricao { get; set; }
    
    /// <summary>
    /// ID da solicitação
    /// </summary>
    public int SolicitacaoId { get; set; }
    
    /// <summary>
    /// Solicitação relacionada
    /// </summary>
    public virtual Solicitacao Solicitacao { get; set; } = null!;
}