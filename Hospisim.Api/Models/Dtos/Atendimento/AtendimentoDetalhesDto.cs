using Hospisim.Api.Dtos.Exame;
using Hospisim.Api.Dtos.Prescricao;

namespace Hospisim.Api.Dtos.Atendimento
{
    public class AtendimentoDetalhesDto
    {
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string Local { get; set; }

        // Dados resumidos das entidades relacionadas
        public Guid PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeProfissional { get; set; }
        public Guid ProntuarioId { get; set; }

        // Listas de DTOs para evitar ciclos
        public List<PrescricaoDto> Prescricoes { get; set; }
        public List<ExameDto> Exames { get; set; }
    }
}