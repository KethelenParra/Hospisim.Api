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
        }
    }
}