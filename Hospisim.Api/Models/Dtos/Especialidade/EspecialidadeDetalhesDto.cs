using Hospisim.Api.Models.Dtos.Profissionais;

namespace Hospisim.Api.Models.Dtos.Especialidade
{
    // DTO completo para ver os detalhes de UMA especialidade, incluindo seus profissionais.
    public class EspecialidadeDetalhesDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int QuantidadeProfissionais { get; set; }
        public List<ProfissionalResumoDto> Profissionais { get; set; }
    }
}