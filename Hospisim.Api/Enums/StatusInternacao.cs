using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum StatusInternacao
    {
        [Display(Name = "Ativa")]
        Ativa = 1,

        [Display(Name = "Alta Concedida")]
        AltaConcedida = 2,

        [Display(Name = "Transferido")]
        Transferido = 3,

        [Display(Name = "Óbito")]
        Obito = 4
    }
}
