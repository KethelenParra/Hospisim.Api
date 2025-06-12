namespace Hospisim.Api.Dtos.Exame
{
    public class ExameDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public string Resultado { get; set; }

        // Contexto do Atendimento
        public Guid AtendimentoId { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string NomePaciente { get; set; }
        public string NomeProfissional { get; set; }
    }
}