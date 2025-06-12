namespace Hospisim.Api.Dtos.Prescricao
{
    public class PrescricaoDto
    {
        public Guid Id { get; set; }
        public string Medicamento { get; set; }
        public string? Dosagem { get; set; }
        public string? Frequencia { get; set; }
        public string ViaAdministracao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Observacoes { get; set; }
        public string StatusPrescricao { get; set; }
        public string? ReacoesAdversas { get; set; }

        public Guid AtendimentoId { get; set; }
        public Guid PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public Guid ProfissionalId { get; set; }
        public string NomeProfissional { get; set; }
    }
}