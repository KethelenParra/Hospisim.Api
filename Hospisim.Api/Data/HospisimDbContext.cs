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

            // ** 1) Pacientes **
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
                },
                new Paciente
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    NomeCompleto = "Daniel Oliveira",
                    CPF = "45678901234",
                    DataNascimento = new DateTime(1985, 3, 15),
                    Sexo = Sexo.Masculino,
                    TipoSanguineo = TipoSanguineo.ABNegativo,
                    Telefone = "(41) 97654-3210",
                    Email = "daniel.oliveira@exemplo.com",
                    EnderecoCompleto = "Rua dos Limoeiros, 50, Centro",
                    NumeroCartaoSUS = "5566778899",
                    EstadoCivil = EstadoCivil.Viuvo,
                    PossuiPlanoSaude = true
                },
                new Paciente
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    NomeCompleto = "Elaine Pereira",
                    CPF = "56789012345",
                    DataNascimento = new DateTime(1992, 7, 22),
                    Sexo = Sexo.Feminino,
                    TipoSanguineo = TipoSanguineo.ONegativo,
                    Telefone = "(51) 91234-9876",
                    Email = "elaine.pereira@exemplo.com",
                    EnderecoCompleto = "Av. Brasil, 200, Bairro União",
                    NumeroCartaoSUS = "6677889900",
                    EstadoCivil = EstadoCivil.Solteiro,
                    PossuiPlanoSaude = false
                }
            );

            // ** 2) Especialidades **
            modelBuilder.Entity<Especialidade>().HasData(
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"), Nome = "Cardiologia" },
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"), Nome = "Pediatria" },
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003"), Nome = "Ortopedia" },
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000004"), Nome = "Dermatologia" },
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000005"), Nome = "Neurologia" },
                new Especialidade { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000006"), Nome = "Ginecologia" }
            );

            // ** 3) Profissionais de Saúde **
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
                },
                new ProfissionalSaude
                {
                    Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000003"),
                    NomeCompleto = "Dr. Lucas Fernandes",
                    CPF = "33344455566",
                    Email = "lucas.fernandes@hospisim.com",
                    Telefone = "(31)91234-0003",
                    RegistroConselho = "34567-CRM",
                    TipoRegistro = TipoRegistro.CRM,
                    EspecialidadeId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003"),
                    DataAdmissao = new DateTime(2020, 11, 10),
                    CargaHorariaSemanal = 35,
                    Turno = Turno.Noite,
                    Ativo = true
                },
                new ProfissionalSaude
                {
                    Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000004"),
                    NomeCompleto = "Dra. Paula Ribeiro",
                    CPF = "44455566677",
                    Email = "paula.ribeiro@hospisim.com",
                    Telefone = "(41)98765-0004",
                    RegistroConselho = "45678-CRM",
                    TipoRegistro = TipoRegistro.CRM,
                    EspecialidadeId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000004"),
                    DataAdmissao = new DateTime(2019, 2, 5),
                    CargaHorariaSemanal = 45,
                    Turno = Turno.Manha,
                    Ativo = false
                }
            );

            // ** 4) Prontuários **
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
                },
                new Prontuario
                {
                    Id = Guid.Parse("cccccccc-0000-0000-0000-000000000003"),
                    Numero = "PRT-1003",
                    DataAbertura = new DateTime(2025, 6, 10, 9, 15, 0),
                    Observacoes = "Prontuário de Carla no retorno",
                    PacienteId = Guid.Parse("33333333-3333-3333-3333-333333333333")
                },
                new Prontuario
                {
                    Id = Guid.Parse("cccccccc-0000-0000-0000-000000000004"),
                    Numero = "PRT-1004",
                    DataAbertura = new DateTime(2025, 6, 12, 11, 45, 0),
                    Observacoes = "Prontuário de Daniel Oliveira",
                    PacienteId = Guid.Parse("44444444-4444-4444-4444-444444444444")
                }
            );

            // ** 5) Atendimentos **
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
                },
                new Atendimento
                {
                    Id = Guid.Parse("dddddddd-0000-0000-0000-000000000003"),
                    DataHora = new DateTime(2025, 6, 10, 15, 30, 0),
                    Tipo = TipoAtendimento.Consulta,
                    Status = StatusAtendimento.EmAndamento,
                    Local = "Consultório 3",
                    PacienteId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000003"),
                    ProntuarioId = Guid.Parse("cccccccc-0000-0000-0000-000000000003")
                },
                new Atendimento
                {
                    Id = Guid.Parse("dddddddd-0000-0000-0000-000000000004"),
                    DataHora = new DateTime(2025, 6, 12, 9, 0, 0),
                    Tipo = TipoAtendimento.Consulta,
                    Status = StatusAtendimento.Cancelado,
                    Local = "Sala 02",
                    PacienteId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000004"),
                    ProntuarioId = Guid.Parse("cccccccc-0000-0000-0000-000000000004")
                }
            );

            // ** 6) Prescrições **
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
                },
                new Prescricao
                {
                    Id = Guid.Parse("eeeeeeee-0000-0000-0000-000000000002"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000003"),
                    ProfissionalId = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000003"),
                    Medicamento = "Paracetamol 750mg",
                    Dosagem = "1 comprimido",
                    Frequencia = "8/8h",
                    ViaAdministracao = ViaAdministracao.Oral,
                    DataInicio = new DateTime(2025, 6, 10, 15, 30, 0),
                    DataFim = null,
                    Observacoes = "Tomar após refeição",
                    StatusPrescricao = StatusPrescricao.Ativa,
                    ReacoesAdversas = ""
                }
            );

            // ** 7) Exames **
            modelBuilder.Entity<Exame>().HasData(
                new Exame
                {
                    Id = Guid.Parse("ffffffff-0000-0000-0000-000000000001"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000001"),
                    Tipo = "Sangue",
                    DataSolicitacao = new DateTime(2025, 7, 5, 10, 0, 0),
                    DataRealizacao = new DateTime(2025, 6, 6, 11, 0, 0),
                    Resultado = "Hemograma normal"
                },
                new Exame
                {
                    Id = Guid.Parse("ffffffff-0000-0000-0000-000000000002"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000004"),
                    Tipo = "Raio-X",
                    DataSolicitacao = new DateTime(2025, 6, 12, 9, 0, 0),
                    DataRealizacao = new DateTime(2025, 6, 12, 10, 30, 0),
                    Resultado = "Sem fraturas"
                }
            );

            // ** 8) Internações **
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
                },
                new Internacao
                {
                    Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaa0002"),
                    PacienteId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    AtendimentoId = Guid.Parse("dddddddd-0000-0000-0000-000000000003"),
                    DataEntrada = new DateTime(2025, 6, 10, 15, 30, 0),
                    PrevisaoAlta = new DateTime(2025, 6, 14, 15, 30, 0),
                    MotivoInternacao = "Tratamento de pneumonia",
                    Leito = "L-20",
                    Quarto = "Q-5",
                    Setor = "Pulmonar",
                    PlanoSaudeUtilizado = "SulAmérica",
                    ObservacoesClinicas = "Oxigenoterapia",
                    StatusInternacao = StatusInternacao.Ativa
                }
            );

            // ** 9) Altas Hospitalares **
            modelBuilder.Entity<AltaHospitalar>().HasData(
                new AltaHospitalar
                {
                    Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbb0001"),
                    InternacaoId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaa0001"),
                    Data = new DateTime(2025, 6, 3, 14, 30, 0),
                    CondicaoPaciente = "Estável",
                    InstrucoesPosAlta = "Repouso e fisioterapia"
                },
                new AltaHospitalar
                {
                    Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                    InternacaoId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaa0002"),
                    Data = new DateTime(2025, 6, 14, 16, 0, 0),
                    CondicaoPaciente = "Melhorado",
                    InstrucoesPosAlta = "Usar máscara de oxigênio em casa"
                }
             );

        }
    }
}