using System.ComponentModel.DataAnnotations;

namespace PYBWeb.Domain.Entities;

/// <summary>
/// Representa uma configuração do sistema VB6 armazenada em arquivo INI
/// </summary>
public class IniConfiguration : BaseEntity
{
    /// <summary>
    /// Nome do arquivo INI (ex: PYBK00VW.INI, PYBK02MW.INI)
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string NomeArquivo { get; set; } = string.Empty;
    
    /// <summary>
    /// Seção do arquivo INI
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Secao { get; set; } = string.Empty;
    
    /// <summary>
    /// Chave da configuração
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Chave { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da configuração
    /// </summary>
    [MaxLength(500)]
    public string? Valor { get; set; }
    
    /// <summary>
    /// Descrição do que representa esta configuração
    /// </summary>
    [MaxLength(200)]
    public string? Descricao { get; set; }
    
    /// <summary>
    /// Tipo da configuração (String, Int, Bool, Path, etc.)
    /// </summary>
    [MaxLength(50)]
    public string TipoConfiguracao { get; set; } = "String";
    
    /// <summary>
    /// Indica se a configuração é crítica para o sistema
    /// </summary>
    public bool IsCritica { get; set; }
}