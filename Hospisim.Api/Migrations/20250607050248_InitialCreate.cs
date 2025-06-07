using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospisim.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoSanguineo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnderecoCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCartaoSUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossuiPlanoSaude = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Id", "CPF", "DataNascimento", "Email", "EnderecoCompleto", "EstadoCivil", "NomeCompleto", "NumeroCartaoSUS", "PossuiPlanoSaude", "Sexo", "Telefone", "TipoSanguineo" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "12345678901", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ana.silva@gmail.com", "Rua das Flores, 123, Bairro Rosas", "Solteiro", "Ana Silva", "9876543210", true, "Feminino", "(11) 91234-5678", "APositivo" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "23456789012", new DateTime(1975, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "bruno.costa@gmail.com", "Av. Central, 456, Centro", "Casado", "Bruno Costa", "1234509876", false, "Masculino", "(21) 98765-4321", "ONegativo" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "34567890123", new DateTime(1990, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "carla.souza@gmail.com", "Praça das Árvores, 789, Bairro Verde", "Divorciado", "Carla Souza", "1122334455", true, "Feminino", "(31) 99876-5432", "BPositivo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
