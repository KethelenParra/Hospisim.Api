using System.ComponentModel.DataAnnotations;
using Hospisim.Api.Enums;

namespace Hospisim.Api.Models.Dtos.Profissionais
{
    // DTO para receber os dados na atualização de um profissional.
    public class UpdateProfissionalDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, MinimumLength = 3)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required(ErrorMessage = "O registro do conselho é obrigatório.")]
        public string RegistroConselho { get; set; }

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        public Guid EspecialidadeId { get; set; }

        public int CargaHorariaSemanal { get; set; }

        [Required(ErrorMessage = "O turno é obrigatório.")]
        public Turno Turno { get; set; }

        [Required(ErrorMessage = "O status 'Ativo' é obrigatório.")]
        public bool Ativo { get; set; }
    }
}