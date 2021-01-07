using ScouTeams.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Zastep
    {
        [Display(Name = "Indeks zastępu")]
        public int ZastepId { get; set; }

        [Display(Name = "Nazwa zastępu")]
        public string Name { get; set; }

        [Display(Name = "Indeks drużyny")]
        public int DruzynaId { get; set; }

        [InverseProperty("Zasteps")]
        public Druzyna Druzyna { get; set; }

        public virtual ICollection<UserZastep> Scouts { get; set; }
    }
}
