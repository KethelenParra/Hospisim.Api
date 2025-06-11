using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospisim.Api.Models
{
    public class Especialidade
    {
        public Guid Id { get; set; }
        [Required]
        public string Nome { get; set; }

        // N:1 Profissional
        public ICollection<ProfissionalSaude> Profissionais { get; set; } = new List<ProfissionalSaude>();
    }
}

