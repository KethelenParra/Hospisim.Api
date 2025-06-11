using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Models
{
    public class Prontuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required, Display(Name = "Número do Prontuário")]
        public string Numero { get; set; }

        [Required, Display(Name = "Data de Abertura")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
        ApplyFormatInEditMode = true)]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Observações Gerais")]
        public string Observacoes { get; set; }

        // Chave estrangeira para Paciente
        [Required]
        public Guid PacienteId { get; set; }

        // Navegação
        public Paciente Paciente { get; set; }

        // Um prontuário pode ter vários atendimentos
        public ICollection<Atendimento> Atendimentos { get; set; } = new List<Atendimento>();
    }
}
