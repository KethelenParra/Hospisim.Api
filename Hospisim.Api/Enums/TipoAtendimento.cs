using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum TipoAtendimento
    {
        [Display(Name = "Emergência")]
        Emergencia = 1,

        [Display(Name = "Consulta")]
        Consulta = 2,

        [Display(Name = "Internação")]
        Internacao = 3
    }
}
