using Hospisim.Api.Dtos.Atendimento;

namespace Hospisim.Api.Dtos.Prontuario
{
    public class ProntuarioDetalhesDto
    {
        public Guid Id { get; set; }
        public string Numero { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Observacoes { get; set; }

        // Contexto do Paciente
        public Guid PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public string CpfPaciente { get; set; }

        // Lista de Atendimentos usando o DTO de resumo
        public List<AtendimentoResumoDto> Atendimentos { get; set; }
    }
}