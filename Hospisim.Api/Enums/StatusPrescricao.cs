using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum StatusPrescricao
    {
        [Display(Name = "Ativa")]
        Ativa = 1,

        [Display(Name = "Suspensa")]
        Suspensa = 2,

        [Display(Name = "Encerrada")]
        Encerrada = 3
    }
}
