using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum TipoRegistro
    {
        [Display(Name = "CRM")]
        CRM = 1,

        [Display(Name = "COREN")]
        COREN = 2,

        [Display(Name = "CRP")]
        CRP = 3
    }
}
