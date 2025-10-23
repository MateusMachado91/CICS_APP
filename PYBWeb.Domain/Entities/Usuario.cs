namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa um usuário do sistema
/// </summary>
public class Usuario : BaseEntity
{
    /// <summary>
    /// Login do usuário
    /// </summary>
    public string Login { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Nome { get; set; } = string.Empty;
    
    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Área/departamento do usuário
    /// </summary>
    public string Area { get; set; } = string.Empty;
    
    /// <summary>
    /// Cargo do usuário
    /// </summary>
    public string? Cargo { get; set; }
    
    /// <summary>
    /// Telefone do usuário
    /// </summary>
    public string? Telefone { get; set; }
    
    /// <summary>
    /// Indica se o usuário é administrador
    /// </summary>
    public bool IsAdministrador { get; set; }
    
    /// <summary>
    /// Indica se o usuário pode aprovar solicitações
    /// </summary>
    public bool PodeAprovar { get; set; }
    
    /// <summary>
    /// Data do último login
    /// </summary>
    public DateTime? UltimoLogin { get; set; }
    
    /// <summary>
    /// Solicitações criadas pelo usuário
    /// </summary>
    public virtual ICollection<Solicitacao> SolicitacoesCriadas { get; set; } = new List<Solicitacao>();
}