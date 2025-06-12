namespace Hospisim.Api.Models.Dtos.AltaHospitalar
{
    public class AltaHospitalarDto
    {
        public Guid Id { get; set; }
        public DateTime Data { get; set; }
        public string CondicaoPaciente { get; set; }
        public string InstrucoesPosAlta { get; set; }

        // Contexto da Internação
        public Guid InternacaoId { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataEntradaInternacao { get; set; }
    }
}
