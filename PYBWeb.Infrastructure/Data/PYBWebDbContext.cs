using Microsoft.EntityFrameworkCore;
using PYBWeb.Domain.Entities;
using PYBWeb.Domain.Enums;

namespace PYBWeb.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados da aplicação
/// </summary>
public class PYBWebDbContext : DbContext
{
    public PYBWebDbContext(DbContextOptions<PYBWebDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Ambiente> Ambientes { get; set; }
    public DbSet<Solicitacao> Solicitacoes { get; set; }
    public DbSet<ItemSolicitacao> ItensSolicitacao { get; set; }
    public DbSet<HistoricoSolicitacao> HistoricoSolicitacoes { get; set; }
    public DbSet<AnexoSolicitacao> AnexosSolicitacao { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<IniConfiguration> IniConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Ambiente
        modelBuilder.Entity<Ambiente>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Descricao)
                .HasMaxLength(500);
                
            entity.Property(e => e.Servidor)
                .HasMaxLength(200);
                
            entity.Property(e => e.RegiaoCics)
                .HasMaxLength(50);
                
            entity.Property(e => e.Tipo)
                .HasConversion<int>();
                
            entity.HasIndex(e => e.Nome).IsUnique();
        });

        // Configuração da entidade Solicitacao
        modelBuilder.Entity<Solicitacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(20);
                
            entity.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Descricao)
                .IsRequired()
                .HasMaxLength(2000);
                
            entity.Property(e => e.Justificativa)
                .HasMaxLength(1000);
                
            entity.Property(e => e.TipoTabela)
                .HasConversion<int>();
                
            entity.Property(e => e.Status)
                .HasConversion<int>();
                
            entity.Property(e => e.UsuarioSolicitante)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.AreaSolicitante)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.ResponsavelImplementacao)
                .HasMaxLength(100);
                
            entity.Property(e => e.Observacoes)
                .HasMaxLength(1000);
                
            entity.HasIndex(e => e.Numero).IsUnique();
            
            // Relacionamento com Ambiente
            entity.HasOne(e => e.Ambiente)
                .WithMany(a => a.Solicitacoes)
                .HasForeignKey(e => e.AmbienteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuração da entidade ItemSolicitacao
        modelBuilder.Entity<ItemSolicitacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Acao)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.ValorAtual)
                .HasMaxLength(500);
                
            entity.Property(e => e.NovoValor)
                .HasMaxLength(500);
                
            entity.Property(e => e.ConfiguracaoCics)
                .IsRequired()
                .HasColumnType("nvarchar(max)");
                
            entity.Property(e => e.Comentarios)
                .HasMaxLength(500);
                
            // Relacionamento com Solicitacao
            entity.HasOne(e => e.Solicitacao)
                .WithMany(s => s.Itens)
                .HasForeignKey(e => e.SolicitacaoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade HistoricoSolicitacao
        modelBuilder.Entity<HistoricoSolicitacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.StatusAnterior)
                .HasConversion<int?>();
                
            entity.Property(e => e.StatusNovo)
                .HasConversion<int>();
                
            entity.Property(e => e.Motivo)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Observacoes)
                .HasMaxLength(1000);
                
            entity.Property(e => e.UsuarioAlteracao)
                .IsRequired()
                .HasMaxLength(100);
                
            // Relacionamento com Solicitacao
            entity.HasOne(e => e.Solicitacao)
                .WithMany(s => s.Historico)
                .HasForeignKey(e => e.SolicitacaoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade AnexoSolicitacao
        modelBuilder.Entity<AnexoSolicitacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.NomeArquivo)
                .IsRequired()
                .HasMaxLength(255);
                
            entity.Property(e => e.CaminhoArquivo)
                .IsRequired()
                .HasMaxLength(500);
                
            entity.Property(e => e.TipoMime)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Descricao)
                .HasMaxLength(500);
                
            // Relacionamento com Solicitacao
            entity.HasOne(e => e.Solicitacao)
                .WithMany(s => s.Anexos)
                .HasForeignKey(e => e.SolicitacaoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuração da entidade Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Login)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(e => e.Area)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Cargo)
                .HasMaxLength(100);
                
            entity.Property(e => e.Telefone)
                .HasMaxLength(20);
                
            entity.HasIndex(e => e.Login).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configuração da entidade IniConfiguration
        modelBuilder.Entity<IniConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.NomeArquivo)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Secao)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Chave)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Valor)
                .HasMaxLength(500);
                
            entity.Property(e => e.Descricao)
                .HasMaxLength(200);
                
            entity.Property(e => e.TipoConfiguracao)
                .HasMaxLength(50);
                
            // Índice único para evitar configurações duplicadas
            entity.HasIndex(e => new { e.NomeArquivo, e.Secao, e.Chave }).IsUnique();
        });

        // Configuração de valores padrão para campos de auditoria
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.DataCriacao))
                    .HasDefaultValueSql("GETDATE()");
                    
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.Ativo))
                    .HasDefaultValue(true);
            }
        }

        // Dados iniciais (Seed Data)
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 10, 20, 10, 0, 0);
        
        // Ambientes padrão
        modelBuilder.Entity<Ambiente>().HasData(
            new Ambiente
            {
                Id = 1,
                Nome = "Desenvolvimento",
                Descricao = "Ambiente de desenvolvimento CICS",
                Tipo = TipoAmbiente.Desenvolvimento,
                Servidor = "CICS-DEV",
                Porta = 1433,
                RegiaoCics = "CICSDV01",
                DataCriacao = seedDate,
                UsuarioCriacao = "SYSTEM"
            },
            new Ambiente
            {
                Id = 2,
                Nome = "Teste",
                Descricao = "Ambiente de teste CICS",
                Tipo = TipoAmbiente.Teste,
                Servidor = "CICS-TST",
                Porta = 1433,
                RegiaoCics = "CICSTS01",
                DataCriacao = seedDate,
                UsuarioCriacao = "SYSTEM"
            },
            new Ambiente
            {
                Id = 3,
                Nome = "Homologação",
                Descricao = "Ambiente de homologação CICS",
                Tipo = TipoAmbiente.Homologacao,
                Servidor = "CICS-HML",
                Porta = 1433,
                RegiaoCics = "CICSHM01",
                DataCriacao = seedDate,
                UsuarioCriacao = "SYSTEM"
            },
            new Ambiente
            {
                Id = 4,
                Nome = "Produção",
                Descricao = "Ambiente de produção CICS",
                Tipo = TipoAmbiente.Producao,
                Servidor = "CICS-PRD",
                Porta = 1433,
                RegiaoCics = "CICSPR01",
                DataCriacao = seedDate,
                UsuarioCriacao = "SYSTEM"
            }
        );

        // Usuário administrador padrão
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Login = "admin",
                Nome = "Administrador do Sistema",
                Email = "admin@banrisul.com.br",
                Area = "TI",
                Cargo = "Administrador",
                IsAdministrador = true,
                PodeAprovar = true,
                DataCriacao = seedDate,
                UsuarioCriacao = "SYSTEM"
            }
        );
    }
}