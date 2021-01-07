using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Hufiec
    {
        [Display(Name = "Indeks hufca")]
        public int HufiecId { get; set; }

        [Display(Name = "Nazwa hufca")]
        public string Name { get; set; }

        [Display(Name = "Indeks chorągwi")]
        public int ChoragiewId { get; set; }

        public virtual ICollection<Druzyna> Druzynas { get; set; }

        [InverseProperty("Hufiecs")]
        public Choragiew Choragiew { get; set; }

        public virtual ICollection<UserHufiec> Scouts { get; set; }

    }
}
