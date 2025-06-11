using System;
using Microsoft.EntityFrameworkCore;
using Hospisim.Api.Models;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Data
{
    public class HospisimDbContext : DbContext
    {
        public HospisimDbContext(DbContextOptions<HospisimDbContext> options)
            : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Prontuario> Prontuarios { get; set; }
        public DbSet<Especialidade> Especialidades { get; set; }
        public DbSet<ProfissionalSaude> Profissionais { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Prescricao> Prescricoes { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<Internacao> Internacoes { get; set; }
        public DbSet<AltaHospitalar> AltasHospitalares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de conversão de enum para string (coluna nvarchar)
            modelBuilder.Entity<Paciente>()
                .Property(p => p.Sexo)
                .HasConversion<string>();

            modelBuilder.Entity<Paciente>()
                .Property(p => p.TipoSanguineo)
                .HasConversion<string>();

            modelBuilder.Entity<Paciente>()
                .Property(p => p.EstadoCivil)
                .HasConversion<string>();

            modelBuilder.Entity<ProfissionalSaude>()
                .Property(p => p.Turno)
                .HasConversion<string>();

            modelBuilder.Entity<ProfissionalSaude>()
                .Property(p => p.TipoRegistro)
                .HasConversion<string>();

            modelBuilder.Entity<Atendimento>()
                .Property(a => a.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Atendimento>()
                .Property(a => a.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Prescricao>()
                .Property(p => p.ViaAdministracao)
                .HasConversion<string>();

            modelBuilder.Entity<Prescricao>()
                .Property(p => p.StatusPrescricao)
                .HasConversion<string>();

            modelBuilder.Entity<Internacao>()
                .Property(i => i.StatusInternacao)
                .HasConversion<string>();

            // Paciente 1:N Prontuário
            modelBuilder.Entity<Prontuario>()
                .HasOne(pr => pr.Paciente)
                .WithMany(p => p.Prontuarios)
                .HasForeignKey(pr => pr.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prontuário 1:N Atendimento
            modelBuilder.Entity<Atendimento>()
                 .HasOne(a => a.Prontuario)
                 .WithMany(pr => pr.Atendimentos)
                 .HasForeignKey(a => a.ProntuarioId)
                 .OnDelete(DeleteBehavior.NoAction);

            // Profissional 1:N Atendimento
            modelBuilder.Entity<Atendimento>()
                .HasOne(a => a.Profissional) 
                .WithMany(p => p.Atendimentos)
                .HasForeignKey(a => a.ProfissionalId)
                .OnDelete(DeleteBehavior.Restrict);

            // Paciente 1:N Atendimento
            modelBuilder.Entity<Atendimento>()
                .HasOne(a => a.Paciente)
                .WithMany() 
                .HasForeignKey(a => a.PacienteId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Atendimento 1:N Prescrição
            modelBuilder.Entity<Prescricao>()
                .HasOne(pr => pr.Atendimento)
                .WithMany(a => a.Prescricoes)
                .HasForeignKey(pr => pr.AtendimentoId);

            // Atendimento 1:N Exame
            modelBuilder.Entity<Exame>()
                .HasOne(ex => ex.Atendimento)
                .WithMany(a => a.Exames)
                .HasForeignKey(ex => ex.AtendimentoId);

            // Atendimento 0..1:1 Internação
            modelBuilder.Entity<Internacao>()
                .HasOne(i => i.Atendimento)
                .WithOne(a => a.Internacao)
                .HasForeignKey<Internacao>(i => i.AtendimentoId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Relacionamento direto Paciente -> Internação
            modelBuilder.Entity<Internacao>()
                .HasOne(i => i.Paciente)
                .WithMany() 
                .HasForeignKey(i => i.PacienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // Internação 0..1:1 Alta Hospitalar
            modelBuilder.Entity<AltaHospitalar>()
                .HasOne(ah => ah.Internacao)
                .WithOne(i => i.AltaHospitalar)
                .HasForeignKey<AltaHospitalar>(ah => ah.InternacaoId);

            // Profissional N:1 Especialidade
            modelBuilder.Entity<ProfissionalSaude>()
                .HasOne(prof => prof.Especialidade)
                .WithMany(e => e.Profissionais)
                .HasForeignKey(prof => prof.EspecialidadeId);

            modelBuilder.Entity<Paciente>().HasData(
                new Paciente
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    NomeCompleto = "Ana Silva",
                    CPF = "12345678901",
                    DataNascimento = new DateTime(1980, 1, 1),
                    Sexo = Sexo.Feminino,
                    TipoSanguineo = TipoSanguineo.APositivo,
                    Telefone = "(11) 91234-5678",
                    Email = "ana.silva@gmail.com",
                    EnderecoCompleto = "Rua das Flores, 123, Bairro Rosas",
                    NumeroCartaoSUS = "9876543210",
                    EstadoCivil = EstadoCivil.Solteiro,
                    PossuiPlanoSaude = true
                },
                new Paciente
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    NomeCompleto = "Bruno Costa",
                    CPF = "23456789012",
                    DataNascimento = new DateTime(1975, 5, 20),
                    Sexo = Sexo.Masculino,
                    TipoSanguineo = TipoSanguineo.ONegativo,
                    Telefone = "(21) 98765-4321",
                    Email = "bruno.costa@gmail.com",
                    EnderecoCompleto = "Av. Central, 456, Centro",
                    NumeroCartaoSUS = "1234509876",
                    EstadoCivil = EstadoCivil.Casado,
                    PossuiPlanoSaude = false
                },
                new Paciente
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    NomeCompleto = "Carla Souza",
                    CPF = "34567890123",
                    DataNascimento = new DateTime(1990, 12, 10),
                    Sexo = Sexo.Feminino,
                    TipoSanguineo = TipoSanguineo.BPositivo,
                    Telefone = "(31) 99876-5432",
                    Email = "carla.souza@gmail.com",
                    EnderecoCompleto = "Praça das Árvores, 789, Bairro Verde",
                    NumeroCartaoSUS = "1122334455",
                    EstadoCivil = EstadoCivil.Divorciado,
                    PossuiPlanoSaude = true
                }
            );

            // 2) Especialidades
            modelBuilder.Entity<Especialidade>().HasData(
                new Especialidade
                {
                    Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
                    Nome = "Cardiologia"
                },
                new Especialidade
                {
                    Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"),
                    Nome = "Pediatria"
                },
                new Especialidade
                {
                    Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003"),
                    Nome = "Ortopedia"
                }
            );

            // 3) Profissionais de Saúde
            modelBuilder.Entity<ProfissionalSaude>().HasData(
                new ProfissionalSaude
                {
                    Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
                    NomeCompleto = "Dra. Marina Souza",
                    CPF = "11122233344",
                    Email = "marina.souza@hospisim.com",
                    Telefone = "(11)91234-0001",
                    RegistroConselho = "12345-CRM",
                    TipoRegistro = TipoRegistro.CRM,
                    EspecialidadeId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
                    DataAdmissao = new DateTime(2022, 3, 1),
                    CargaHorariaSemanal = 40,
                    Turno = Turno.Manha,
                    Ativo = true
                },
                new ProfissionalSaude
                {
                    Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
                    NomeCompleto = "Dr. Rafael Lima",
                    CPF = "22233344455",
                    Email = "rafael.lima@hospisim.com",
                    Telefone = "(21)98765-0002",
                    RegistroConselho = "67890-CRM",
                    TipoRegistro = TipoRegistro.CRM,
                    EspecialidadeId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"),
                    DataAdmissao = new DateTime(2021, 7, 15),
                    CargaHorariaSemanal = 30,
                    Turno = Turno.Tarde,
                    Ativo = true
                }
            );

            // 4) Prontuários
            modelBuilder.Entity<Prontuario>().HasData(
                new Prontuario
                {
                    Id = Guid.Parse("cccccccc-0000-0000-0000-000000000001"),
                    Numero = "PRT-1001",
                    DataAbertura = new DateTime(2025, 6, 1, 14, 30, 0),
                    Observacoes = "Prontuário inicial de Ana Silva",
                    PacienteId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                },
                new Prontuario
                {
                    Id = Guid.Parse("cccccccc-0000-0000-0000-000000000002"),
                    Numero = "PRT-1002",
                    DataAbertura = new DateTime(2025, 6, 5, 10, 0, 0),
                    Observacoes = "Prontuário de Bruno Costa",
                    PacienteId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                }
            );

            // 5) Atendimentos
            modelBuilder.Entity<Atendimento>().HasData(
                new Atendimento
                {
                    Id = Guid.Parse("dddddddd-0000-0000-0000-000000000001"),
                    DataHora = new DateTime(2025, 6, 5, 10, 0, 0),
                    Tipo = TipoAtendimento.Consulta,
                    Status = StatusAtendimento.Realizado,
                    Local = "Sala 01",
                    PacienteId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
                    ProntuarioId = Guid.Parse("cccccccc-0000-0000-0000-000000000001")
                },
                new Atendimento
                {
                    Id = Guid.Parse("dddddddd-0000-0000-0000-000000000002"),
                    DataHora = new DateTime(2025, 6, 5, 10, 0, 0),
                    Tipo = TipoAtendimento.Emergencia,
                    Status = StatusAtendimento.EmAndamento,
                    Local = "Emergência",
                    PacienteId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
                    ProntuarioId = Guid.Parse("cccccccc-0000-0000-0000-000000000002")
                }
            );

            // 6) Prescrições
            modelBuilder.Entity<Prescricao>().HasData(
                new Prescricao
                {
                    Id = Guid.Parse("eeeeeeee-0000-0000-0000-000000000001"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000001"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
                    Medicamento = "Dipirona 500mg",
                    Dosagem = "1 comprimido",
                    Frequencia = "12/12h",
                    ViaAdministracao = ViaAdministracao.Oral,
                    DataInicio = new DateTime(2025, 6, 5, 10, 0, 0),
                    DataFim = null,
                    Observacoes = "",                   
                    StatusPrescricao = StatusPrescricao.Ativa,
                    ReacoesAdversas = ""                  
                }
            );

            // 7) Exames
            modelBuilder.Entity<Exame>().HasData(
                new Exame
                {
                    Id = Guid.Parse("ffffffff-0000-0000-0000-000000000001"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000001"),
                    Tipo = "Sangue",
                    DataSolicitacao = new DateTime(2025, 7, 5, 10, 0, 0),
                    DataRealizacao = new DateTime(2025, 6, 5, 10, 0, 0),
                    Resultado = "Hemograma normal"
                }
            );

            // 8) Internações
            modelBuilder.Entity<Internacao>().HasData(
                new Internacao
                {
                    Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                    PacienteId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000002"),
                    DataEntrada = new DateTime(2025, 6, 1, 14, 30, 0),
                    PrevisaoAlta = new DateTime(2025, 6, 9, 14, 30, 0),
                    MotivoInternacao = "Observação pós-cirurgia",
                    Leito = "L-15",
                    Quarto = "Q-3",
                    Setor = "Clínica Geral",
                    PlanoSaudeUtilizado = "Unimed",
                    ObservacoesClinicas = "Sem complicações",
                    StatusInternacao = StatusInternacao.Ativa
                }
            );

            // 9) Altas Hospitalares
            modelBuilder.Entity<AltaHospitalar>().HasData(
                new AltaHospitalar
                {
                    Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbb0001"),
                    InternacaoId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                    Data = new DateTime(2025, 6, 3, 14, 30, 0),
                    CondicaoPaciente = "Estável",
                    InstrucoesPosAlta = "Repouso e fisioterapia"
                }
            );

        }
    }
}