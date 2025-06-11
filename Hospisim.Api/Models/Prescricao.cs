using System;
using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Models
{
    public class Prescricao
    {
        public Guid Id { get; set; }

        [Required]
        public Guid AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }

        [Required]
        public Guid ProfissionalId { get; set; }
        public ProfissionalSaude Profissional { get; set; }

        [Required, Display(Name = "Medicamento")]
        public string Medicamento { get; set; }

        [Display(Name = "Dosagem")]
        public string? Dosagem { get; set; }

        [Display(Name = "Frequência")]
        public string? Frequencia { get; set; }

        [Display(Name = "Via de Administração")]
        public ViaAdministracao ViaAdministracao { get; set; } 

        [Required, Display(Name = "Data Início")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime? DataFim { get; set; }

        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Status da Prescrição")]
        public StatusPrescricao StatusPrescricao { get; set; } 

        [Display(Name = "Reações Adversas")]
        public string? ReacoesAdversas { get; set; }
    }
}
