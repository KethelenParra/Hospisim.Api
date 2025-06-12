using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Models.Dtos.Profissionais
{
    // DTO para receber os dados na criação de um profissional.
    public class CreateProfissionalDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, MinimumLength = 3)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter 11 dígitos.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required(ErrorMessage = "O registro do conselho é obrigatório.")]
        public string RegistroConselho { get; set; }

        [Required(ErrorMessage = "O tipo de registro é obrigatório.")]
        public TipoRegistro TipoRegistro { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        public Guid EspecialidadeId { get; set; }

        [Required(ErrorMessage = "A data de admissão é obrigatória.")]
        public DateTime DataAdmissao { get; set; }

        public int CargaHorariaSemanal { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório.")]
        public Turno Turno { get; set; }
    }
}