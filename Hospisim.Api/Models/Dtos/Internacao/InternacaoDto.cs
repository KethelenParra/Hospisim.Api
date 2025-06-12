using Hospisim.Api.Models.Dtos.AltaHospitalar;

namespace Hospisim.Api.Dtos.Internacao
{
    // DTO para exibir os detalhes completos de uma internação.
    public class InternacaoDto
    {
        public Guid Id { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? PrevisaoAlta { get; set; }
        public string MotivoInternacao { get; set; }
        public string Leito { get; set; }
        public string Quarto { get; set; }
        public string Setor { get; set; }
        public string PlanoSaudeUtilizado { get; set; }
        public string ObservacoesClinicas { get; set; }
        public string StatusInternacao { get; set; }

        // Contexto do Atendimento e Paciente
        public Guid AtendimentoId { get; set; }
        public Guid PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public string CpfPaciente { get; set; }

        public AltaHospitalarDto Alta { get; set; } 
    }
}