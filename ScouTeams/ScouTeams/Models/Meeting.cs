using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class Meeting
    {
        [Display(Name = "Indeks zbiórki")]
        public int MeetingId { get; set; }

        [Display(Name = "Indeks zastępu")]
        public int ZastepId { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Indeks zastępowego")]
        public string ScoutId { get; set; }

        public virtual ICollection<ScoutPresence> ScoutPresences { get; set; }

    }
}
