using Hospisim.Api.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hospisim.Api.Models
{
    public class Paciente
    {
        public Guid Id { get; set; }
        [Required, Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }
        [Required, Display(Name = "CPF")]
        public string CPF { get; set; }
        [Required, Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataNascimento { get; set; }
        [Display(Name = "Sexo")]
        public Sexo Sexo { get; set; }
        [Display(Name = "Tipo Sanguineo")]
        public TipoSanguineo TipoSanguineo { get; set; }
        [Required, Phone, Display(Name = "Telefone")]
        public string Telefone { get; set; }
        [EmailAddress, Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Endereço Completo")]
        public string EnderecoCompleto { get; set; }
        [Required, Display(Name = "Nº Cartão do SUS")]
        public string NumeroCartaoSUS { get; set; }
        [Display(Name = "Estado civil")]
        public EstadoCivil EstadoCivil { get; set; }

        [Required, Display(Name = "Possui Plano de Saúde?")]
        public bool PossuiPlanoSaude { get; set; }

        // Relacionamento 1:N com Prontuario (será adicionado depois)
        [ValidateNever]
        [JsonIgnore]
        public ICollection<Prontuario> Prontuarios { get; set; }
    }
}
