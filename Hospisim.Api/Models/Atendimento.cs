using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Models
{
    public class Atendimento
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Data e hora do atendimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataHora { get; set; }
        [Required, Display(Name = "Tipo de Atendimento")]
        public TipoAtendimento Tipo { get; set; }
        [Required, Display(Name = "Status")]
        public StatusAtendimento Status { get; set; }
        public string Local { get; set; }

        // FKs
        [Required]
        public Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        [Required]
        public Guid ProfissionalId { get; set; }
        public ProfissionalSaude Profissional { get; set; }

        [Required]
        public Guid ProntuarioId { get; set; }
        public Prontuario Prontuario { get; set; }

        // 1:N Prescrições
        public ICollection<Prescricao> Prescricoes { get; set; } = new List<Prescricao>();

        // 1:N Exames
        public ICollection<Exame> Exames { get; set; } = new List<Exame>();

        // 0..1:1 Internação
        public Internacao Internacao { get; set; }
    }
}
