using System;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Models
{
    public class Exame
    {
        public Guid Id { get; set; }
        [Required]
        public Guid AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }
        public string Tipo { get; set; }

        [Required, Display(Name = "Data de Solicitação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataSolicitacao { get; set; }

        [Required, Display(Name = "Data da Realização")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime? DataRealizacao { get; set; }
        public string Resultado { get; set; }
    }
}
