using System;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Models
{
    public class AltaHospitalar
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid InternacaoId { get; set; }
        public Internacao Internacao { get; set; }

        [Required, Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Required, Display(Name = "Condição do Paciente")]
        public string CondicaoPaciente { get; set; }
        [Required, Display(Name = "Instruções Pós Alta")]
        public string InstrucoesPosAlta { get; set; }
    }
}
