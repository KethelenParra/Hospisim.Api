using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Dtos.Prontuario
{
    public class CreateProntuarioDto
    {
        [Required(ErrorMessage = "O ID do Paciente é obrigatório para criar um prontuário.")]
        public Guid PacienteId { get; set; }

        public string Observacoes { get; set; }
    }
}