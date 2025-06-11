using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum StatusAtendimento
    {
        [Display(Name = "Realizado")]
        Realizado = 1,

        [Display(Name = "Em Andamento")]
        EmAndamento = 2,

        [Display(Name = "Cancelado")]
        Cancelado = 3
    }
}
