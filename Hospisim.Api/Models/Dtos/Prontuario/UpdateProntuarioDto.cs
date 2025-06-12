namespace Hospisim.Api.Dtos.Prontuario
{
    public class UpdateProntuarioDto
    {
        // O único campo que faz sentido atualizar em um prontuário são as observações gerais.
        public string Observacoes { get; set; }
    }
}