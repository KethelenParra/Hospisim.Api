using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum Turno
    {
        [Display(Name = "Manhã")]
        Manha = 1,

        [Display(Name = "Tarde")]
        Tarde = 2,

        [Display(Name = "Noite")]
        Noite = 3
    }
}
