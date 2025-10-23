using PYBWeb.Domain.Enums;

namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa um ambiente CICS no sistema
/// </summary>
public class Ambiente : BaseEntity
{
    /// <summary>
    /// Nome do ambiente
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Descrição do ambiente
    /// </summary>
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo do ambiente
    /// </summary>
    public TipoAmbiente Tipo { get; set; }
    
    /// <summary>
    /// Servidor onde o ambiente está hospedado
    /// </summary>
    public string Servidor { get; set; } = string.Empty;
    
    /// <summary>
    /// Porta de conexão do ambiente
    /// </summary>
    public int? Porta { get; set; }
    
    /// <summary>
    /// Região CICS do ambiente
    /// </summary>
    public string RegiaoCics { get; set; } = string.Empty;
    
    /// <summary>
    /// Solicitações relacionadas a este ambiente
    /// </summary>
    public virtual ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();
}