using ScouTeams.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public class ScoutViewModel
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Funkcja")]
        public string Functions { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Data urodzenia")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Opłacone składki")]
        public bool PaidContributions { get; set; }

        [Display(Name = "Stopień harcerski")]
        public ScoutDegree ScoutDegree { get; set; }

        [Display(Name = "Stopień instruktorski")]
        public InstructorDegree InstructorDegree { get; set; }

    }
}
