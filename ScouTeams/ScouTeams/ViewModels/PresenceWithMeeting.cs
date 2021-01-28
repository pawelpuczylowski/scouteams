using ScouTeams.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public class PresenceWithMeeting
    {
        [Display(Name = "Indeks obecności")]
        public int ScoutPresenceId { get; set; }

        [Display(Name = "Indeks użytkownika")]
        public string ScoutId { get; set; }

        [Display(Name = "Indeks zbiórki")]
        public int MeetingId { get; set; }

        [Display(Name = "Imię")]
        public string ScoutName { get; set; }

        [Display(Name = "Nazwisko")]
        public string ScoutSurname { get; set; }

        [Display(Name = "Aktywność na zbiórce")]
        public Presence Presence { get; set; }
    }
}
