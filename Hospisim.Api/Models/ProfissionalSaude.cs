using Hospisim.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Hospisim.Api.Models
{
    public class ProfissionalSaude
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }
        [Required, Display(Name = "CPF")]
        public string CPF { get; set; }
        [EmailAddress, Display(Name = "E-mail")]
        public string Email { get; set; }
        [Phone, Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [Display(Name = "Registro Conselho")]
        public string RegistroConselho { get; set; }
        [Display(Name = "Tipo Registro")]
        public TipoRegistro TipoRegistro { get; set; }
        [Required, Display(Name = "Especialidade")]
        public Guid EspecialidadeId { get; set; }
        public Especialidade Especialidade { get; set; }
        [Display(Name = "Data Admissão")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataAdmissao { get; set; }
        [Display(Name = "Carga Horária Semanal")]
        public int CargaHorariaSemanal { get; set; }
        [Display(Name = "Turno")]
        public Turno Turno { get; set; }
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        // relacionamento 1:N com atendimentos
        public ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();
    }
}
