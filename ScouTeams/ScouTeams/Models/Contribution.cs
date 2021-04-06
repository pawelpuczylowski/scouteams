using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Contribution
    {
        [Display(Name = "Indeks składki")]
        public int ContributionId { get; set; }

        [Display(Name = "Indeks harcerza")]
        public string ScoutId { get; set; }

        [Display(Name = "Kwota")]
        public float Amount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }
    }
}
