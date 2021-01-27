using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Druzyna
    {
        [Key]
        [Display(Name = "Indeks drużyny")]
        public int DruzynaId { get; set; }

        [Display(Name = "Nazwa drużyny")]
        public string Name { get; set; }

        [Display(Name = "Indeks hufca")]
        public int HufiecId { get; set; }

        public virtual ICollection<Zastep> Zasteps { get; set; }

        [InverseProperty("Druzynas")]
        public Hufiec Hufiec { get; set; }

        public virtual ICollection<UserDruzyna> Scouts { get; set; }
    }
}
