using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum ViaAdministracao
    {
        [Display(Name = "Oral")]
        Oral = 1,

        [Display(Name = "Intravenosa")]
        Intravenosa = 2,

        [Display(Name = "Intramuscular")]
        Intramuscular = 3,

        [Display(Name = "Subcutânea")]
        Subcutanea = 4,

        [Display(Name = "Tópica")]
        Topica = 5
    }
}
