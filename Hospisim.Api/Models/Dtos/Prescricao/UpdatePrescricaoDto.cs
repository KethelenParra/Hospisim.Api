using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Dtos.Prescricao
{
    public class UpdatePrescricaoDto
    {
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

        public DateTime? DataFim { get; set; }

        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "O Status da Prescrição é obrigatório.")]
        public StatusPrescricao StatusPrescricao { get; set; }

        public string? ReacoesAdversas { get; set; }
    }
}