using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Enums
{
    public enum TipoSanguineo
    {
        [Display(Name = "A+")] 
        APositivo = 1,

        [Display(Name = "A-")] 
        ANegativo = 2,

        [Display(Name = "B+")] 
        BPositivo = 3,

        [Display(Name = "B-")] 
        BNegativo = 4,

        [Display(Name = "O+")] 
        OPositivo = 5,

        [Display(Name = "O-")] 
        ONegativo = 6,

        [Display(Name = "AB+")] 
        ABPositivo = 7,

        [Display(Name = "AB-")] 
        ABNegativo = 8
    }
}
