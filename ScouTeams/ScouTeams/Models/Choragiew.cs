using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Choragiew
    {
        [Display(Name = "Indeks chorągwi")]
        public int ChoragiewId { get; set; }

        [Display(Name = "Nazwa chorągwi")]
        public string Name { get; set; }

        [Display(Name = "Indeks kwatery głównej")]
        public int KwateraGlownaId { get; set; }

        public virtual ICollection<Hufiec> Hufiecs { get; set; }

        [InverseProperty("Choragiews")]
        public KwateraGlowna KwateraGlowna { get; set; }

        public virtual ICollection<UserChoragiew> Scouts { get; set; }
    }
}
