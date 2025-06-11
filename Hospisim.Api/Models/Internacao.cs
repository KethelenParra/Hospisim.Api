using System;
using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Models
{
    public class Internacao
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; }
        [Required]
        public Guid AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }
        [Required, Display(Name = "Data de Entrada")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataEntrada { get; set; }
        [Required, Display(Name = "Data Privisão de Alta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime? PrevisaoAlta { get; set; }
        [Required, Display(Name = "Motivo Da Internação")]
        public string MotivoInternacao { get; set; }
        public string Leito { get; set; }
        public string Quarto { get; set; }
        public string Setor { get; set; }
        [Required, Display(Name = "Plano de Saúde Utilizado")]
        public string PlanoSaudeUtilizado { get; set; }
        [Display(Name = "Observações Clínicas")]
        public string ObservacoesClinicas { get; set; }
        [Display(Name = "Status da Internação")]
        public StatusInternacao StatusInternacao { get; set; }
        [Display(Name = "Data de Alta")]
        // 0..1:1 AltaHospitalar
        public AltaHospitalar AltaHospitalar { get; set; }
    }
}
