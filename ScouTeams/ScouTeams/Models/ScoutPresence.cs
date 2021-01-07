using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public enum Presence
    {
        [Display(Name = "Obecność")] Attending, [Display(Name = "Spóźnienie")] Late, [Display(Name = "Nieobecność")] Absent
    }

    public class ScoutPresence
    {
        [Display(Name = "Indeks obecności")]
        public int ScoutPresenceId { get; set; }

        [Display(Name = "Indeks użytkownika")]
        public string ScoutId { get; set; }

        [Display(Name = "Indeks zbiórki")]
        public int MeetingId { get; set; }

        [Display(Name = "Aktywność na zbiórce")]
        public Presence Presence { get; set; }

        [InverseProperty("ScoutPresences")]
        public Meeting Meeting { get; set; }
    }
}
