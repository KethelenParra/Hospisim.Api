using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Dtos.Internacao
{
    public class CreateInternacaoDto
    {
        [Required(ErrorMessage = "O ID do Atendimento é obrigatório.")]
        public Guid AtendimentoId { get; set; }

        [Required(ErrorMessage = "O motivo da internação é obrigatório.")]
        [StringLength(200)]
        public string MotivoInternacao { get; set; }

        [Required(ErrorMessage = "A previsão de alta é obrigatória.")]
        public DateTime PrevisaoAlta { get; set; }

        [Required(ErrorMessage = "O setor é obrigatório.")]
        public string Setor { get; set; }

        [Required(ErrorMessage = "O quarto é obrigatório.")]
        public string Quarto { get; set; }

        [Required(ErrorMessage = "O leito é obrigatório.")]
        public string Leito { get; set; }

        public string PlanoSaudeUtilizado { get; set; }
        public string ObservacoesClinicas { get; set; }
    }
}