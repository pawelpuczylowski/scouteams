using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScouTeams.Models
{
    public class KwateraGlowna
    {
        [Display(Name = "Indeks kwatery głównej")]
        public int KwateraGlownaId { get; set; }
        public virtual ICollection<Choragiew> Choragiews { get; set; }
        public virtual ICollection<Scout> Scouts { get; set; }

    }
}
