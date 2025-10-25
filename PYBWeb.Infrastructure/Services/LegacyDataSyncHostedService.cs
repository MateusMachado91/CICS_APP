using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PYBWeb.Domain.Interfaces;

namespace PYBWeb.Infrastructure.Services;

/// <summary>
/// Serviço em background para sincronização automática dos dados legados na inicialização
/// </summary>
public class LegacyDataSyncHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LegacyDataSyncHostedService> _logger;

    public LegacyDataSyncHostedService(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<LegacyDataSyncHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Aguarda um tempo para garantir que a aplicação inicializou completamente
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        var autoSync = _configuration.GetValue<bool>("LegacySystem:AutoSyncOnStartup");
        
        if (!autoSync)
        {
            _logger.LogInformation("Sincronização automática desabilitada");
            return;
        }

        _logger.LogInformation("Iniciando sincronização automática dos dados legados");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            
            // Sincroniza arquivos INI
            var iniService = scope.ServiceProvider.GetRequiredService<IIniConfigurationService>();
            var iniSyncResult = await iniService.SincronizarConfiguracoesAsync();
            
            if (iniSyncResult)
            {
                _logger.LogInformation("Sincronização de arquivos INI concluída com sucesso");
            }
            else
            {
                _logger.LogWarning("Sincronização de arquivos INI não encontrou dados ou falhou");
            }

            // Migra dados legados
            var migrationService = scope.ServiceProvider.GetRequiredService<ILegacyDataMigrationService>();
            
            // Primeiro analisa a estrutura
            var analise = await migrationService.AnalisarEstruturaBancosAsync();
            _logger.LogInformation("Análise da estrutura dos bancos legados: {Analise}", 
                System.Text.Json.JsonSerializer.Serialize(analise, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));

            // Executa a migração se houver dados
            if (!analise.ContainsKey("erro"))
            {
                var migrationResult = await migrationService.ExecutarMigracaoCompletaAsync();
                
                if (migrationResult)
                {
                    _logger.LogInformation("Migração de dados legados concluída com sucesso");
                    
                    // Valida os dados migrados
                    var validacao = await migrationService.ValidarDadosMigradosAsync();
                    _logger.LogInformation("Validação dos dados migrados: {Validacao}", 
                        System.Text.Json.JsonSerializer.Serialize(validacao));
                }
                else
                {
                    _logger.LogWarning("Migração de dados legados não encontrou dados ou falhou");
                }
            }
            else
            {
                _logger.LogError("Erro na análise dos bancos legados: {Erro}", analise["erro"]);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro na sincronização automática dos dados legados");
        }

        _logger.LogInformation("Sincronização automática finalizada");
    }
}