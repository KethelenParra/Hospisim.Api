namespace Hospisim.Api.Models.Dtos.Profissionais
{
    // DTO super simples para listar profissionais dentro de uma especialidade, sem causar ciclos.
    public class ProfissionalResumoDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string RegistroConselho { get; set; }
    }
}