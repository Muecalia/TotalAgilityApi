using Microsoft.EntityFrameworkCore;
using TotalAgilityApi.Domain.Queries.Responses;

namespace TotalAgilityApi.Infraestrutura.Context
{
    public class TotalAgilityContext: DbContext
    {
        //public TotalAgilityContext() { }

        public TotalAgilityContext(DbContextOptions<TotalAgilityContext> options) : base(options) { }

        //Agentes
        public DbSet<TopAgentesValidacaoResponse> TopAgentesValidacao { get; set; }

        public DbSet<ActividadesManuaisResponse> ActividadesManuais { get; set; }
        public DbSet<EstadosDataBasesResponse> EstadosDataBases { get; set; }
        public DbSet<NumerosEsperaImagensResponse> NumerosEsperaImagens { get; set; }
        public DbSet<NumerosRemovidosResponse> NumerosRemovidos { get; set; }
        public DbSet<NumerosTerminadosResponse> NumerosTerminados { get; set; }
        public DbSet<ProcessosSuspensosResponse> ProcessosSuspensos { get; set; }
        public DbSet<ProcessosSuspensosCategoriasResponse> EstatisticaProcessosSuspensos { get; set; }
        public DbSet<ProcessosPendentesResponse> ProcessosPendentes { get; set; }
        
        public DbSet<ProcessosSuspensosMsisdnResponse> ProcessosSuspensosMsisdn { get; set; }
        public DbSet<ProcessosDuplicadosResponse> ProcessosDuplicados { get; set; }
        public DbSet<ProcessosMais7DiasResponse> ProcessosMais7Dias { get; set; }
        public DbSet<ProcessosMais7DiasItensResponse> ProcessosMais7DiasItens { get; set; }
        public DbSet<ProcessosCursoTerminadosResponse> ProcessosCursoTerminados { get; set; }
        public DbSet<ProcessosValidadosDiaResponse> ProcessosValidadosDia { get; set; }
        public DbSet<ProcessosRecebidosDiaResponse> ProcessosRecebidosDia { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            //modelBuilder.Entity<TopAgentesValidacaoResponse>(entity =>
            //{
            //    entity.HasNoKey(); // Indica que a entidade não possui uma chave primária
            //    entity.ToView(null); // Indica que não é uma view, é uma stored procedure
            //});
        //}

    }
}
