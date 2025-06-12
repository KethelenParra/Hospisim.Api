namespace Hospisim.Api.Dtos.Atendimento
{
    // DTO com informações resumidas de um atendimento para ser exibido na lista do prontuário.
    public class AtendimentoResumoDto
    {
        public Guid Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }
        public string NomeProfissional { get; set; }
    }
}