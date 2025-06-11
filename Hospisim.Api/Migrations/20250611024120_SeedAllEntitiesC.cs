using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospisim.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllEntitiesC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prontuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prontuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prontuarios_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profissionais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistroConselho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoRegistro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EspecialidadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CargaHorariaSemanal = table.Column<int>(type: "int", nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profissionais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profissionais_Especialidades_EspecialidadeId",
                        column: x => x.EspecialidadeId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Atendimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfissionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProntuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Prontuarios_ProntuarioId",
                        column: x => x.ProntuarioId,
                        principalTable: "Prontuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtendimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataRealizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Resultado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exames_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Internacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtendimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrevisaoAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MotivoInternacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quarto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Setor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanoSaudeUtilizado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObservacoesClinicas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusInternacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internacoes_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Internacoes_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prescricoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AtendimentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfissionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Medicamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Frequencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViaAdministracao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusPrescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReacoesAdversas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescricoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescricoes_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescricoes_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AltasHospitalares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CondicaoPaciente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrucoesPosAlta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AltasHospitalares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AltasHospitalares_Internacoes_InternacaoId",
                        column: x => x.InternacaoId,
                        principalTable: "Internacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Especialidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), "Cardiologia" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000002"), "Pediatria" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000003"), "Ortopedia" }
                });

            migrationBuilder.InsertData(
                table: "Prontuarios",
                columns: new[] { "Id", "DataAbertura", "Numero", "Observacoes", "PacienteId" },
                values: new object[,]
                {
                    { new Guid("cccccccc-0000-0000-0000-000000000001"), new DateTime(2025, 6, 1, 14, 30, 0, 0, DateTimeKind.Unspecified), "PRT-1001", "Prontuário inicial de Ana Silva", new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("cccccccc-0000-0000-0000-000000000002"), new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "PRT-1002", "Prontuário de Bruno Costa", new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "Profissionais",
                columns: new[] { "Id", "Ativo", "CPF", "CargaHorariaSemanal", "DataAdmissao", "Email", "EspecialidadeId", "NomeCompleto", "RegistroConselho", "Telefone", "TipoRegistro", "Turno" },
                values: new object[,]
                {
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), true, "11122233344", 40, new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "marina.souza@hospisim.com", new Guid("aaaaaaaa-0000-0000-0000-000000000001"), "Dra. Marina Souza", "12345-CRM", "(11)91234-0001", "CRM", "Manha" },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000002"), true, "22233344455", 30, new DateTime(2021, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "rafael.lima@hospisim.com", new Guid("aaaaaaaa-0000-0000-0000-000000000002"), "Dr. Rafael Lima", "67890-CRM", "(21)98765-0002", "CRM", "Tarde" }
                });

            migrationBuilder.InsertData(
                table: "Atendimentos",
                columns: new[] { "Id", "DataHora", "Local", "PacienteId", "ProfissionalId", "ProntuarioId", "Status", "Tipo" },
                values: new object[,]
                {
                    { new Guid("dddddddd-0000-0000-0000-000000000001"), new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Sala 01", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("bbbbbbbb-0000-0000-0000-000000000001"), new Guid("cccccccc-0000-0000-0000-000000000001"), "Realizado", "Consulta" },
                    { new Guid("dddddddd-0000-0000-0000-000000000002"), new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Emergência", new Guid("33333333-3333-3333-3333-333333333333"), new Guid("bbbbbbbb-0000-0000-0000-000000000002"), new Guid("cccccccc-0000-0000-0000-000000000002"), "EmAndamento", "Emergencia" }
                });

            migrationBuilder.InsertData(
                table: "Exames",
                columns: new[] { "Id", "AtendimentoId", "DataRealizacao", "DataSolicitacao", "Resultado", "Tipo" },
                values: new object[] { new Guid("ffffffff-0000-0000-0000-000000000001"), new Guid("dddddddd-0000-0000-0000-000000000001"), new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "Hemograma normal", "Sangue" });

            migrationBuilder.InsertData(
                table: "Internacoes",
                columns: new[] { "Id", "AtendimentoId", "DataEntrada", "Leito", "MotivoInternacao", "ObservacoesClinicas", "PacienteId", "PlanoSaudeUtilizado", "PrevisaoAlta", "Quarto", "Setor", "StatusInternacao" },
                values: new object[] { new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaa0001"), new Guid("dddddddd-0000-0000-0000-000000000002"), new DateTime(2025, 6, 1, 14, 30, 0, 0, DateTimeKind.Unspecified), "L-15", "Observação pós-cirurgia", "Sem complicações", new Guid("22222222-2222-2222-2222-222222222222"), "Unimed", new DateTime(2025, 6, 9, 14, 30, 0, 0, DateTimeKind.Unspecified), "Q-3", "Clínica Geral", "Ativa" });

            migrationBuilder.InsertData(
                table: "Prescricoes",
                columns: new[] { "Id", "AtendimentoId", "DataFim", "DataInicio", "Dosagem", "Frequencia", "Medicamento", "Observacoes", "ProfissionalId", "ReacoesAdversas", "StatusPrescricao", "ViaAdministracao" },
                values: new object[] { new Guid("eeeeeeee-0000-0000-0000-000000000001"), new Guid("dddddddd-0000-0000-0000-000000000001"), null, new DateTime(2025, 6, 5, 10, 0, 0, 0, DateTimeKind.Unspecified), "1 comprimido", "12/12h", "Dipirona 500mg", "", new Guid("bbbbbbbb-0000-0000-0000-000000000001"), "", "Ativa", "Oral" });

            migrationBuilder.InsertData(
                table: "AltasHospitalares",
                columns: new[] { "Id", "CondicaoPaciente", "Data", "InstrucoesPosAlta", "InternacaoId" },
                values: new object[] { new Guid("22222222-bbbb-bbbb-bbbb-bbbbbbbb0001"), "Estável", new DateTime(2025, 6, 3, 14, 30, 0, 0, DateTimeKind.Unspecified), "Repouso e fisioterapia", new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaa0001") });

            migrationBuilder.CreateIndex(
                name: "IX_AltasHospitalares_InternacaoId",
                table: "AltasHospitalares",
                column: "InternacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_PacienteId",
                table: "Atendimentos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_ProfissionalId",
                table: "Atendimentos",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_ProntuarioId",
                table: "Atendimentos",
                column: "ProntuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_AtendimentoId",
                table: "Exames",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Internacoes_AtendimentoId",
                table: "Internacoes",
                column: "AtendimentoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Internacoes_PacienteId",
                table: "Internacoes",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescricoes_AtendimentoId",
                table: "Prescricoes",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescricoes_ProfissionalId",
                table: "Prescricoes",
                column: "ProfissionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Profissionais_EspecialidadeId",
                table: "Profissionais",
                column: "EspecialidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Prontuarios_PacienteId",
                table: "Prontuarios",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AltasHospitalares");

            migrationBuilder.DropTable(
                name: "Exames");

            migrationBuilder.DropTable(
                name: "Prescricoes");

            migrationBuilder.DropTable(
                name: "Internacoes");

            migrationBuilder.DropTable(
                name: "Atendimentos");

            migrationBuilder.DropTable(
                name: "Profissionais");

            migrationBuilder.DropTable(
                name: "Prontuarios");

            migrationBuilder.DropTable(
                name: "Especialidades");
        }
    }
}
