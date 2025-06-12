using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Dtos.AltaHospitalar
{
    public class UpdateAltaHospitalarDto
    {
        [Required(ErrorMessage = "A condição do paciente na alta é obrigatória.")]
        [StringLength(200)]
        public string CondicaoPaciente { get; set; }

        [Required(ErrorMessage = "As instruções pós-alta são obrigatórias.")]
        public string InstrucoesPosAlta { get; set; }
    }
}