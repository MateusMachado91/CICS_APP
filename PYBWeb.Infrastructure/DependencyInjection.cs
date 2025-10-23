using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PYBWeb.Domain.Interfaces;
using PYBWeb.Infrastructure.Data;
using PYBWeb.Infrastructure.Repositories;

namespace PYBWeb.Infrastructure;

/// <summary>
/// Configuração da injeção de dependência da infraestrutura
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuração do Entity Framework
        services.AddDbContext<PYBWebDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(PYBWebDbContext).Assembly.FullName)));

        // Registro dos repositórios
        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
        services.AddScoped<IAmbienteRepository, AmbienteRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }
}