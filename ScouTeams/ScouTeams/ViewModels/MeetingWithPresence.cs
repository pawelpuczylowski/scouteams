using ScouTeams.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public class MeetingWithPresence
    {
        [Display(Name = "Indeks zbiórki")]
        public int MeetingId { get; set; }

        [Display(Name = "Zastęp")]
        public string ZastepName { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Aktywność na zbiórce")]
        public Presence Presence { get; set; }
    }
}
