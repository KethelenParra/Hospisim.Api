using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Dtos.Atendimento
{
    public class CreateAtendimentoDto
    {
        [Required(ErrorMessage = "O ID do Paciente é obrigatório.")]
        public Guid PacienteId { get; set; }

        [Required(ErrorMessage = "O ID do Profissional é obrigatório.")]
        public Guid ProfissionalId { get; set; }

        [Required(ErrorMessage = "O Tipo de Atendimento é obrigatório.")]
        public TipoAtendimento Tipo { get; set; }

        [Required]
        [StringLength(100)]
        public string Local { get; set; }
    }
}