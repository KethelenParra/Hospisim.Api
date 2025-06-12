using Hospisim.Api.Models.Dtos.Especialidade;

namespace Hospisim.Api.Models.Dtos.Profissionais
{
    public class ProfissionalDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string CpfFormatado { get; set; } // Enviamos o CPF já formatado
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string RegistroConselho { get; set; }
        public string TipoRegistro { get; set; } // Enviamos o nome do Enum, não o número
        public bool Ativo { get; set; }
        public int CargaHorariaSemanal { get; set; }
        public string Turno { get; set; }
        public DateTime DataAdmissao { get; set; }
        public EspecialidadeDto Especialidade { get; set; } // Usa o DTO de Especialidade
    }
}
