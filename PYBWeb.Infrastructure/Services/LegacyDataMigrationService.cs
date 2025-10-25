using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;

namespace PYBWeb.Infrastructure.Services;

/// <summary>
/// Serviço para migração de dados dos bancos SQLite legados
/// </summary>
public class LegacyDataMigrationService : ILegacyDataMigrationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LegacyDataMigrationService> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ISolicitacaoRepository _solicitacaoRepository;
    private readonly IAmbienteRepository _ambienteRepository;

    public LegacyDataMigrationService(
        IConfiguration configuration,
        ILogger<LegacyDataMigrationService> logger,
        IUsuarioRepository usuarioRepository,
        ISolicitacaoRepository solicitacaoRepository,
        IAmbienteRepository ambienteRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _solicitacaoRepository = solicitacaoRepository;
        _ambienteRepository = ambienteRepository;
    }

    /// <summary>
    /// Analisa a estrutura dos bancos SQLite legados
    /// </summary>
    public async Task<Dictionary<string, object>> AnalisarEstruturaBancosAsync()
    {
        var resultado = new Dictionary<string, object>();
        
        try
        {
            var pastaData = _configuration.GetValue<string>("PastaData") ?? "DATA";
            var caminhoCompleto = Path.GetFullPath(pastaData);

            if (!Directory.Exists(caminhoCompleto))
            {
                resultado["erro"] = $"Pasta DATA não encontrada: {caminhoCompleto}";
                return resultado;
            }

            // Analisa banco colaboradores.db
            var colaboradoresDb = Path.Combine(caminhoCompleto, "colaboradores.db");
            if (File.Exists(colaboradoresDb))
            {
                resultado["colaboradores"] = await AnalisarBancoAsync(colaboradoresDb);
            }

            // Analisa banco dados2025.db
            var dados2025Db = Path.Combine(caminhoCompleto, "dados2025.db");
            if (File.Exists(dados2025Db))
            {
                resultado["dados2025"] = await AnalisarBancoAsync(dados2025Db);
            }

            _logger.LogInformation("Análise da estrutura dos bancos legados concluída");
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao analisar estrutura dos bancos legados");
            resultado["erro"] = ex.Message;
            return resultado;
        }
    }

    /// <summary>
    /// Importa dados dos usuários/colaboradores do banco legado
    /// </summary>
    public async Task<bool> ImportarUsuariosAsync()
    {
        try
        {
            var pastaData = _configuration.GetValue<string>("PastaData") ?? "DATA";
            var colaboradoresDb = Path.Combine(Path.GetFullPath(pastaData), "colaboradores.db");

            if (!File.Exists(colaboradoresDb))
            {
                _logger.LogWarning("Banco colaboradores.db não encontrado");
                return false;
            }

            var connectionString = $"Data Source={colaboradoresDb}";
            using var context = new LegacyDataContext(connectionString);

            // Primeiro, vamos descobrir as tabelas disponíveis
            var tabelas = await context.ObterNomesTabelasAsync();
            _logger.LogInformation("Tabelas encontradas no banco colaboradores: {Tabelas}", string.Join(", ", tabelas));

            // Busca por tabelas que possam conter usuários
            var possiveisTabelasUsuarios = tabelas.Where(t => 
                t.ToLower().Contains("user") || 
                t.ToLower().Contains("usuario") || 
                t.ToLower().Contains("colaborador") ||
                t.ToLower().Contains("people") ||
                t.ToLower().Contains("pessoa")).ToList();

            if (!possiveisTabelasUsuarios.Any())
            {
                // Se não encontrou tabelas específicas, tenta a primeira tabela
                possiveisTabelasUsuarios = tabelas.Take(1).ToList();
            }

            var usuariosImportados = 0;

            foreach (var nomeTabela in possiveisTabelasUsuarios)
            {
                var estrutura = await context.ObterEstruturaTabelaAsync(nomeTabela);
                var dados = await context.ExecutarConsultaAsync($"SELECT * FROM {nomeTabela}");

                foreach (var linha in dados)
                {
                    try
                    {
                        var usuario = await CriarUsuarioFromLegacyData(linha);
                        if (usuario != null)
                        {
                            // Verifica se usuário já existe
                            var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(usuario.Email);
                            if (usuarioExistente == null)
                            {
                                await _usuarioRepository.AddAsync(usuario);
                                usuariosImportados++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Erro ao importar usuário: {Erro}", ex.Message);
                    }
                }
            }

            _logger.LogInformation("Importação de usuários concluída: {UsuariosImportados} usuários", usuariosImportados);
            return usuariosImportados > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao importar usuários do banco legado");
            return false;
        }
    }

    /// <summary>
    /// Importa dados das solicitações do banco legado
    /// </summary>
    public async Task<bool> ImportarSolicitacoesAsync()
    {
        try
        {
            var pastaData = _configuration.GetValue<string>("PastaData") ?? "DATA";
            var dados2025Db = Path.Combine(Path.GetFullPath(pastaData), "dados2025.db");

            if (!File.Exists(dados2025Db))
            {
                _logger.LogWarning("Banco dados2025.db não encontrado");
                return false;
            }

            var connectionString = $"Data Source={dados2025Db}";
            using var context = new LegacyDataContext(connectionString);

            var tabelas = await context.ObterNomesTabelasAsync();
            _logger.LogInformation("Tabelas encontradas no banco dados2025: {Tabelas}", string.Join(", ", tabelas));

            // Busca por tabelas que possam conter solicitações
            var possiveisTabelasSolicitacoes = tabelas.Where(t => 
                t.ToLower().Contains("solicit") || 
                t.ToLower().Contains("request") || 
                t.ToLower().Contains("pedido") ||
                t.ToLower().Contains("ticket")).ToList();

            if (!possiveisTabelasSolicitacoes.Any())
            {
                possiveisTabelasSolicitacoes = tabelas.Take(1).ToList();
            }

            var solicitacoesImportadas = 0;

            foreach (var nomeTabela in possiveisTabelasSolicitacoes)
            {
                var dados = await context.ExecutarConsultaAsync($"SELECT * FROM {nomeTabela}");

                foreach (var linha in dados)
                {
                    try
                    {
                        var solicitacao = await CriarSolicitacaoFromLegacyData(linha);
                        if (solicitacao != null)
                        {
                            await _solicitacaoRepository.AddAsync(solicitacao);
                            solicitacoesImportadas++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Erro ao importar solicitação: {Erro}", ex.Message);
                    }
                }
            }

            _logger.LogInformation("Importação de solicitações concluída: {SolicitacoesImportadas} solicitações", solicitacoesImportadas);
            return solicitacoesImportadas > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao importar solicitações do banco legado");
            return false;
        }
    }

    /// <summary>
    /// Executa migração completa dos dados legados
    /// </summary>
    public async Task<bool> ExecutarMigracaoCompletaAsync()
    {
        _logger.LogInformation("Iniciando migração completa dos dados legados");

        var sucessoUsuarios = await ImportarUsuariosAsync();
        var sucessoSolicitacoes = await ImportarSolicitacoesAsync();

        var migracaoSucesso = sucessoUsuarios || sucessoSolicitacoes;
        
        if (migracaoSucesso)
        {
            _logger.LogInformation("Migração completa concluída com sucesso");
        }
        else
        {
            _logger.LogWarning("Migração completa não encontrou dados para importar");
        }

        return migracaoSucesso;
    }

    /// <summary>
    /// Valida a integridade dos dados migrados
    /// </summary>
    public async Task<Dictionary<string, object>> ValidarDadosMigradosAsync()
    {
        var resultado = new Dictionary<string, object>();

        try
        {
            var totalUsuarios = await _usuarioRepository.ContarAsync();
            var totalSolicitacoes = await _solicitacaoRepository.ContarAsync();
            var totalAmbientes = await _ambienteRepository.ContarAsync();

            resultado["usuarios_total"] = totalUsuarios;
            resultado["solicitacoes_total"] = totalSolicitacoes;
            resultado["ambientes_total"] = totalAmbientes;
            resultado["validacao_sucesso"] = true;

            _logger.LogInformation("Validação concluída - Usuários: {Usuarios}, Solicitações: {Solicitacoes}, Ambientes: {Ambientes}",
                totalUsuarios, totalSolicitacoes, totalAmbientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na validação dos dados migrados");
            resultado["erro"] = ex.Message;
            resultado["validacao_sucesso"] = false;
        }

        return resultado;
    }

    /// <summary>
    /// Analisa um banco SQLite específico
    /// </summary>
    private async Task<Dictionary<string, object>> AnalisarBancoAsync(string caminhoDb)
    {
        var resultado = new Dictionary<string, object>();
        
        try
        {
            var connectionString = $"Data Source={caminhoDb}";
            using var context = new LegacyDataContext(connectionString);

            var tabelas = await context.ObterNomesTabelasAsync();
            resultado["tabelas"] = tabelas;

            var estruturas = new Dictionary<string, object>();
            foreach (var tabela in tabelas)
            {
                var estrutura = await context.ObterEstruturaTabelaAsync(tabela);
                var contagem = await context.ExecutarConsultaAsync($"SELECT COUNT(*) as total FROM {tabela}");
                
                estruturas[tabela] = new
                {
                    colunas = estrutura,
                    total_registros = contagem.FirstOrDefault()?["total"]
                };
            }

            resultado["estruturas"] = estruturas;
        }
        catch (Exception ex)
        {
            resultado["erro"] = ex.Message;
        }

        return resultado;
    }

    /// <summary>
    /// Cria um objeto Usuario a partir dos dados legados
    /// </summary>
    private async Task<Usuario?> CriarUsuarioFromLegacyData(Dictionary<string, object> dados)
    {
        try
        {
            // Mapeia campos comuns que podem existir nos dados legados
            var login = ObterValorCampo(dados, "login", "user", "usuario", "id");
            var nome = ObterValorCampo(dados, "nome", "name", "usuario", "login");
            var email = ObterValorCampo(dados, "email", "mail", "e_mail");
            var area = ObterValorCampo(dados, "area", "department", "dept", "setor");
            var cargo = ObterValorCampo(dados, "cargo", "position", "funcao", "job");

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(nome))
                return null;

            // Se não tem email, gera um baseado no login
            if (string.IsNullOrEmpty(email))
            {
                email = $"{login}@empresa.com.br";
            }

            return new Usuario
            {
                Login = login,
                Nome = nome,
                Email = email,
                Area = area ?? "Não informado",
                Cargo = cargo,
                IsAdministrador = false,
                PodeAprovar = false,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = "MIGRATION"
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Cria um objeto Solicitacao a partir dos dados legados
    /// </summary>
    private async Task<Solicitacao?> CriarSolicitacaoFromLegacyData(Dictionary<string, object> dados)
    {
        try
        {
            var titulo = ObterValorCampo(dados, "titulo", "title", "assunto", "descricao");
            var descricao = ObterValorCampo(dados, "descricao", "description", "detalhes", "observacao");
            var solicitante = ObterValorCampo(dados, "solicitante", "usuario", "user", "criado_por");
            var area = ObterValorCampo(dados, "area", "department", "setor");

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(solicitante))
                return null;

            // Busca um ambiente padrão
            var ambiente = await _ambienteRepository.GetByIdAsync(1) ?? 
                          (await _ambienteRepository.GetAllAsync()).FirstOrDefault();

            if (ambiente == null)
                return null;

            return new Solicitacao
            {
                Numero = $"LEG-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                Titulo = titulo,
                Descricao = descricao ?? titulo,
                Justificativa = "Migrado do sistema legado",
                TipoTabela = TipoTabela.PCT, // Valor padrão
                Status = StatusSolicitacao.Pendente,
                Prioridade = 3, // Prioridade média
                UsuarioSolicitante = solicitante,
                AreaSolicitante = area ?? "Migração",
                AmbienteId = ambiente.Id,
                DataCriacao = DateTime.Now,
                UsuarioCriacao = "MIGRATION"
            };
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Obtém o valor de um campo dos dados legados, tentando várias possibilidades de nomes
    /// </summary>
    private string? ObterValorCampo(Dictionary<string, object> dados, params string[] possiveisNomes)
    {
        foreach (var nome in possiveisNomes)
        {
            // Tenta o nome exato
            if (dados.ContainsKey(nome) && dados[nome] != null)
                return dados[nome].ToString();

            // Tenta variações com case insensitive
            var chaveEncontrada = dados.Keys.FirstOrDefault(k => 
                string.Equals(k, nome, StringComparison.OrdinalIgnoreCase));

            if (chaveEncontrada != null && dados[chaveEncontrada] != null)
                return dados[chaveEncontrada].ToString();
        }

        return null;
    }
}