using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospisim.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Especialidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000004"), "Dermatologia" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000005"), "Neurologia" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000006"), "Ginecologia" }
                });

            migrationBuilder.UpdateData(
                table: "Exames",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-000000000001"),
                column: "DataRealizacao",
                value: new DateTime(2025, 6, 6, 11, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Pacientes",
                columns: new[] { "Id", "CPF", "DataNascimento", "Email", "EnderecoCompleto", "EstadoCivil", "NomeCompleto", "NumeroCartaoSUS", "PossuiPlanoSaude", "Sexo", "Telefone", "TipoSanguineo" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "45678901234", new DateTime(1985, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "daniel.oliveira@exemplo.com", "Rua dos Limoeiros, 50, Centro", "Viuvo", "Daniel Oliveira", "5566778899", true, "Masculino", "(41) 97654-3210", "ABNegativo" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "56789012345", new DateTime(1992, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "elaine.pereira@exemplo.com", "Av. Brasil, 200, Bairro União", "Solteiro", "Elaine Pereira", "6677889900", false, "Feminino", "(51) 91234-9876", "ONegativo" }
                });

            migrationBuilder.InsertData(
                table: "Profissionais",
                columns: new[] { "Id", "Ativo", "CPF", "CargaHorariaSemanal", "DataAdmissao", "Email", "EspecialidadeId", "NomeCompleto", "RegistroConselho", "Telefone", "TipoRegistro", "Turno" },
                values: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000003"), true, "33344455566", 35, new DateTime(2020, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "lucas.fernandes@hospisim.com", new Guid("aaaaaaaa-0000-0000-0000-000000000003"), "Dr. Lucas Fernandes", "34567-CRM", "(31)91234-0003", "CRM", "Noite" });

            migrationBuilder.InsertData(
                table: "Prontuarios",
                columns: new[] { "Id", "DataAbertura", "Numero", "Observacoes", "PacienteId" },
                values: new object[] { new Guid("cccccccc-0000-0000-0000-000000000003"), new DateTime(2025, 6, 10, 9, 15, 0, 0, DateTimeKind.Unspecified), "PRT-1003", "Prontuário de Carla no retorno", new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.InsertData(
                table: "Atendimentos",
                columns: new[] { "Id", "DataHora", "Local", "PacienteId", "ProfissionalId", "ProntuarioId", "Status", "Tipo" },
                values: new object[] { new Guid("dddddddd-0000-0000-0000-000000000003"), new DateTime(2025, 6, 10, 15, 30, 0, 0, DateTimeKind.Unspecified), "Consultório 3", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("bbbbbbbb-0000-0000-0000-000000000003"), new Guid("cccccccc-0000-0000-0000-000000000003"), "EmAndamento", "Consulta" });

            migrationBuilder.InsertData(
                table: "Profissionais",
                columns: new[] { "Id", "Ativo", "CPF", "CargaHorariaSemanal", "DataAdmissao", "Email", "EspecialidadeId", "NomeCompleto", "RegistroConselho", "Telefone", "TipoRegistro", "Turno" },
                values: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000004"), false, "44455566677", 45, new DateTime(2019, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "paula.ribeiro@hospisim.com", new Guid("aaaaaaaa-0000-0000-0000-000000000004"), "Dra. Paula Ribeiro", "45678-CRM", "(41)98765-0004", "CRM", "Manha" });

            migrationBuilder.InsertData(
                table: "Prontuarios",
                columns: new[] { "Id", "DataAbertura", "Numero", "Observacoes", "PacienteId" },
                values: new object[] { new Guid("cccccccc-0000-0000-0000-000000000004"), new DateTime(2025, 6, 12, 11, 45, 0, 0, DateTimeKind.Unspecified), "PRT-1004", "Prontuário de Daniel Oliveira", new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.InsertData(
                table: "Atendimentos",
                columns: new[] { "Id", "DataHora", "Local", "PacienteId", "ProfissionalId", "ProntuarioId", "Status", "Tipo" },
                values: new object[] { new Guid("dddddddd-0000-0000-0000-000000000004"), new DateTime(2025, 6, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Sala 02", new Guid("44444444-4444-4444-4444-444444444444"), new Guid("bbbbbbbb-0000-0000-0000-000000000004"), new Guid("cccccccc-0000-0000-0000-000000000004"), "Cancelado", "Consulta" });

            migrationBuilder.InsertData(
                table: "Internacoes",
                columns: new[] { "Id", "AtendimentoId", "DataEntrada", "Leito", "MotivoInternacao", "ObservacoesClinicas", "PacienteId", "PlanoSaudeUtilizado", "PrevisaoAlta", "Quarto", "Setor", "StatusInternacao" },
                values: new object[] { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaa0002"), new Guid("dddddddd-0000-0000-0000-000000000003"), new DateTime(2025, 6, 10, 15, 30, 0, 0, DateTimeKind.Unspecified), "L-20", "Tratamento de pneumonia", "Oxigenoterapia", new Guid("33333333-3333-3333-3333-333333333333"), "SulAmérica", new DateTime(2025, 6, 14, 15, 30, 0, 0, DateTimeKind.Unspecified), "Q-5", "Pulmonar", "Ativa" });

            migrationBuilder.InsertData(
                table: "Prescricoes",
                columns: new[] { "Id", "AtendimentoId", "DataFim", "DataInicio", "Dosagem", "Frequencia", "Medicamento", "Observacoes", "ProfissionalId", "ReacoesAdversas", "StatusPrescricao", "ViaAdministracao" },
                values: new object[] { new Guid("eeeeeeee-0000-0000-0000-000000000002"), new Guid("dddddddd-0000-0000-0000-000000000003"), null, new DateTime(2025, 6, 10, 15, 30, 0, 0, DateTimeKind.Unspecified), "1 comprimido", "8/8h", "Paracetamol 750mg", "Tomar após refeição", new Guid("bbbbbbbb-0000-0000-0000-000000000003"), "", "Ativa", "Oral" });

            migrationBuilder.InsertData(
                table: "AltasHospitalares",
                columns: new[] { "Id", "CondicaoPaciente", "Data", "InstrucoesPosAlta", "InternacaoId" },
                values: new object[] { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Melhorado", new DateTime(2025, 6, 14, 16, 0, 0, 0, DateTimeKind.Unspecified), "Usar máscara de oxigênio em casa", new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaa0002") });

            migrationBuilder.InsertData(
                table: "Exames",
                columns: new[] { "Id", "AtendimentoId", "DataRealizacao", "DataSolicitacao", "Resultado", "Tipo" },
                values: new object[] { new Guid("ffffffff-0000-0000-0000-000000000002"), new Guid("dddddddd-0000-0000-0000-000000000004"), new DateTime(2025, 6, 12, 10, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 12, 9, 0, 0, 0, DateTimeKind.Unspecified), "Sem fraturas", "Raio-X" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AltasHospitalares",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbb0002"));

            migrationBuilder.DeleteData(
                table: "Especialidades",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Especialidades",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exames",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Pacientes",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Prescricoes",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Atendimentos",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Internacoes",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaa0002"));

            migrationBuilder.DeleteData(
                table: "Atendimentos",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Profissionais",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Prontuarios",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Especialidades",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Pacientes",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Profissionais",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Prontuarios",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-0000-0000-0000-000000000003"));

            migrationBuilder.UpdateData(
                table: "Exames",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-0000-0000-0000-000000000001"),
                column: "DataRealizacao",
                value: new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
