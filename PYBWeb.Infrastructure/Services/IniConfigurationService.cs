using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Interfaces;
using System.Text;

namespace PYBWeb.Infrastructure.Services;

/// <summary>
/// Serviço para gerenciamento de configurações INI do sistema VB6
/// </summary>
public class IniConfigurationService : IIniConfigurationService
{
    private readonly IIniConfigurationRepository _iniRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IniConfigurationService> _logger;
    
    public IniConfigurationService(
        IIniConfigurationRepository iniRepository,
        IConfiguration configuration,
        ILogger<IniConfigurationService> logger)
    {
        _iniRepository = iniRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Carrega as configurações de um arquivo INI para o banco de dados
    /// </summary>
    public async Task<bool> CarregarArquivoIniAsync(string caminhoArquivo)
    {
        try
        {
            if (!File.Exists(caminhoArquivo))
            {
                _logger.LogWarning("Arquivo INI não encontrado: {CaminhoArquivo}", caminhoArquivo);
                return false;
            }

            var nomeArquivo = Path.GetFileName(caminhoArquivo);
            var linhas = await File.ReadAllLinesAsync(caminhoArquivo, Encoding.GetEncoding("ISO-8859-1"));
            
            string secaoAtual = "";
            var configuracoes = new List<IniConfiguration>();

            foreach (var linha in linhas)
            {
                var linhaTrim = linha.Trim();
                
                // Ignora linhas vazias e comentários
                if (string.IsNullOrEmpty(linhaTrim) || linhaTrim.StartsWith(';') || linhaTrim.StartsWith('#'))
                    continue;

                // Detecta seção [SECAO]
                if (linhaTrim.StartsWith('[') && linhaTrim.EndsWith(']'))
                {
                    secaoAtual = linhaTrim.Substring(1, linhaTrim.Length - 2);
                    continue;
                }

                // Processa chave=valor
                var separatorIndex = linhaTrim.IndexOf('=');
                if (separatorIndex > 0)
                {
                    var chave = linhaTrim.Substring(0, separatorIndex).Trim();
                    var valor = linhaTrim.Substring(separatorIndex + 1).Trim();

                    // Remove aspas se existirem
                    if (valor.StartsWith('"') && valor.EndsWith('"') && valor.Length > 1)
                    {
                        valor = valor.Substring(1, valor.Length - 2);
                    }

                    var configuracao = new IniConfiguration
                    {
                        NomeArquivo = nomeArquivo,
                        Secao = secaoAtual,
                        Chave = chave,
                        Valor = valor,
                        TipoConfiguracao = DeterminarTipoConfiguracao(valor),
                        IsCritica = DeterminarSeCritica(chave),
                        DataCriacao = DateTime.Now,
                        UsuarioCriacao = "SYSTEM"
                    };

                    configuracoes.Add(configuracao);
                }
            }

            // Remove configurações antigas do mesmo arquivo
            var configuracoesExistentes = await _iniRepository.BuscarPorArquivoAsync(nomeArquivo);
            foreach (var config in configuracoesExistentes)
            {
                await _iniRepository.RemoverAsync(config.Id);
            }

            // Adiciona as novas configurações
            foreach (var config in configuracoes)
            {
                await _iniRepository.AdicionarAsync(config);
            }

            _logger.LogInformation("Arquivo INI carregado com sucesso: {NomeArquivo}, {QuantidadeConfiguracoes} configurações",
                nomeArquivo, configuracoes.Count);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar arquivo INI: {CaminhoArquivo}", caminhoArquivo);
            return false;
        }
    }

    /// <summary>
    /// Obtém o valor de uma configuração específica
    /// </summary>
    public async Task<string?> ObterValorConfiguracaoAsync(string nomeArquivo, string secao, string chave)
    {
        var configuracao = await _iniRepository.BuscarConfiguracaoAsync(nomeArquivo, secao, chave);
        return configuracao?.Valor;
    }

    /// <summary>
    /// Obtém todas as configurações de um arquivo
    /// </summary>
    public async Task<Dictionary<string, Dictionary<string, string>>> ObterTodasConfiguracoesAsync(string nomeArquivo)
    {
        var configuracoes = await _iniRepository.BuscarPorArquivoAsync(nomeArquivo);
        var resultado = new Dictionary<string, Dictionary<string, string>>();

        foreach (var config in configuracoes)
        {
            if (!resultado.ContainsKey(config.Secao))
            {
                resultado[config.Secao] = new Dictionary<string, string>();
            }

            resultado[config.Secao][config.Chave] = config.Valor ?? "";
        }

        return resultado;
    }

    /// <summary>
    /// Atualiza uma configuração específica
    /// </summary>
    public async Task<bool> AtualizarConfiguracaoAsync(string nomeArquivo, string secao, string chave, string novoValor)
    {
        return await _iniRepository.AtualizarValorConfiguracaoAsync(nomeArquivo, secao, chave, novoValor);
    }

    /// <summary>
    /// Exporta as configurações atuais para um arquivo INI
    /// </summary>
    public async Task<bool> ExportarParaArquivoIniAsync(string nomeArquivo, string caminhoDestino)
    {
        try
        {
            var configuracoes = await ObterTodasConfiguracoesAsync(nomeArquivo);
            var conteudo = new StringBuilder();

            foreach (var secao in configuracoes)
            {
                conteudo.AppendLine($"[{secao.Key}]");
                
                foreach (var config in secao.Value)
                {
                    conteudo.AppendLine($"{config.Key}={config.Value}");
                }
                
                conteudo.AppendLine(); // Linha em branco entre seções
            }

            await File.WriteAllTextAsync(caminhoDestino, conteudo.ToString(), Encoding.GetEncoding("ISO-8859-1"));
            
            _logger.LogInformation("Configurações exportadas para: {CaminhoDestino}", caminhoDestino);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao exportar configurações para arquivo: {CaminhoDestino}", caminhoDestino);
            return false;
        }
    }

    /// <summary>
    /// Sincroniza todas as configurações dos arquivos INI da pasta DATA
    /// </summary>
    public async Task<bool> SincronizarConfiguracoesAsync()
    {
        try
        {
            var pastaData = _configuration.GetValue<string>("PastaData") ?? "DATA";
            var caminhoCompleto = Path.GetFullPath(pastaData);

            if (!Directory.Exists(caminhoCompleto))
            {
                _logger.LogWarning("Pasta DATA não encontrada: {CaminhoCompleto}", caminhoCompleto);
                return false;
            }

            var arquivosIni = Directory.GetFiles(caminhoCompleto, "*.ini", SearchOption.TopDirectoryOnly);
            var sucessos = 0;

            foreach (var arquivo in arquivosIni)
            {
                if (await CarregarArquivoIniAsync(arquivo))
                {
                    sucessos++;
                }
            }

            _logger.LogInformation("Sincronização concluída: {Sucessos}/{Total} arquivos processados", 
                sucessos, arquivosIni.Length);

            return sucessos > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na sincronização das configurações INI");
            return false;
        }
    }

    /// <summary>
    /// Determina o tipo da configuração baseado no valor
    /// </summary>
    private static string DeterminarTipoConfiguracao(string valor)
    {
        if (string.IsNullOrEmpty(valor))
            return "String";

        if (bool.TryParse(valor, out _))
            return "Boolean";

        if (int.TryParse(valor, out _))
            return "Integer";

        if (double.TryParse(valor, out _))
            return "Decimal";

        if (Path.IsPathRooted(valor) || valor.Contains("\\") || valor.Contains("/"))
            return "Path";

        return "String";
    }

    /// <summary>
    /// Determina se uma configuração é crítica baseado na chave
    /// </summary>
    private static bool DeterminarSeCritica(string chave)
    {
        var chavesCriticas = new[]
        {
            "DATABASE", "DB", "CONNECTION", "SERVER", "HOST", "PORT", "PASSWORD", "USER",
            "CICS", "REGION", "SISTEMA", "VERSAO", "PATH", "DIRETORIO"
        };

        return chavesCriticas.Any(c => chave.ToUpper().Contains(c));
    }
}