using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Dtos.Exame
{
    public class CreateExameDto
    {
        [Required(ErrorMessage = "O ID do atendimento é obrigatório.")]
        public Guid AtendimentoId { get; set; }

        [Required(ErrorMessage = "O tipo do exame é obrigatório.")]
        [StringLength(100)]
        public string Tipo { get; set; }
    }
}