using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Dtos.Exame
{
    public class UpdateExameResultDto
    {
        [Required(ErrorMessage = "A data de realização do exame é obrigatória.")]
        public DateTime DataRealizacao { get; set; }

        [Required(ErrorMessage = "O resultado do exame é obrigatório.")]
        public string Resultado { get; set; }
    }
}