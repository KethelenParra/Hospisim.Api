using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Dtos.Prescricao
{
    public class CreatePrescricaoDto
    {
        [Required(ErrorMessage = "O ID do Atendimento é obrigatório.")]
        public Guid AtendimentoId { get; set; }

        [Required(ErrorMessage = "O ID do Profissional que prescreveu é obrigatório.")]
        public Guid ProfissionalId { get; set; }

        [Required(ErrorMessage = "O nome do Medicamento é obrigatório.")]
        [StringLength(150)]
        public string Medicamento { get; set; }

        [Required(ErrorMessage = "A Dosagem é obrigatória.")]
        [StringLength(100)]
        public string Dosagem { get; set; }

        [Required(ErrorMessage = "A Frequência é obrigatória.")]
        [StringLength(100)]
        public string Frequencia { get; set; }

        [Required(ErrorMessage = "A Via de Administração é obrigatória.")]
        public ViaAdministracao ViaAdministracao { get; set; }

        public string? Observacoes { get; set; }
    }
}