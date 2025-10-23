using System.ComponentModel.DataAnnotations;
using PYBWeb.Domain.Enums;

namespace PYBWeb.Web.Models;

/// <summary>
/// Modelo para criação de nova solicitação
/// </summary>
public class NovaSolicitacaoModel
{
    /// <summary>
    /// Título/resumo da solicitação
    /// </summary>
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres")]
    public string Titulo { get; set; } = string.Empty;
    
    /// <summary>
    /// Descrição detalhada da solicitação
    /// </summary>
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres")]
    public string Descricao { get; set; } = string.Empty;
    
    /// <summary>
    /// Justificativa para a solicitação
    /// </summary>
    [Required(ErrorMessage = "A justificativa é obrigatória")]
    [StringLength(1000, ErrorMessage = "A justificativa deve ter no máximo 1000 caracteres")]
    public string Justificativa { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo de tabela CICS afetada
    /// </summary>
    [Required(ErrorMessage = "O tipo de tabela é obrigatório")]
    public TipoTabela? TipoTabela { get; set; }
    
    /// <summary>
    /// Data limite para implementação
    /// </summary>
    public DateTime? DataLimite { get; set; }
    
    /// <summary>
    /// Prioridade da solicitação (1-4)
    /// </summary>
    [Range(1, 4, ErrorMessage = "A prioridade deve estar entre 1 e 4")]
    public int Prioridade { get; set; } = 2;
    
    /// <summary>
    /// Usuário solicitante
    /// </summary>
    [Required(ErrorMessage = "O usuário solicitante é obrigatório")]
    [StringLength(100, ErrorMessage = "O usuário solicitante deve ter no máximo 100 caracteres")]
    public string UsuarioSolicitante { get; set; } = string.Empty;
    
    /// <summary>
    /// Área/departamento solicitante
    /// </summary>
    [Required(ErrorMessage = "A área solicitante é obrigatória")]
    [StringLength(100, ErrorMessage = "A área solicitante deve ter no máximo 100 caracteres")]
    public string AreaSolicitante { get; set; } = string.Empty;
    
    /// <summary>
    /// Observações gerais
    /// </summary>
    [StringLength(1000, ErrorMessage = "As observações devem ter no máximo 1000 caracteres")]
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// ID do ambiente alvo
    /// </summary>
    [Required(ErrorMessage = "O ambiente é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um ambiente válido")]
    public int? AmbienteId { get; set; }
}