using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ScouTeams.Models;

namespace ScouTeams.Areas.Identity.Data
{
    public enum ScoutDegree
    {
        Ochotniczka, Młodzik, Tropicielka, Wywiadowca, Pionierka, Odkrywca, Samarytanka, Ćwik,
        [Display(Name = "Harcerka Orla")] HarcerkaOrla, [Display(Name = "Harcerz Orli")] HarcerzOrli,
        [Display(Name = "Harcerka Rzeczypospolitej")] HarcerkaRzeczypospolitej, [Display(Name = "Harcerz Rzeczypospolitej")] HarcerzRzeczypospolitej
    }
    public enum InstructorDegree
    {
        Przewodnik, Przewodniczka, Podharcmistrz, Podharcmistrzyni, Harcmistrz, Harcmistrzyni
    }

    public class Scout : IdentityUser
    {
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Data urodzenia")]
        public DateTime DateOfBirth { get; set; }

        public KwateraGlowna KwateraGlowna { get; set; }
        public virtual ICollection<UserChoragiew> Choragiews { get; set; }

        public virtual ICollection<UserHufiec> Hufiecs { get; set; }

        public virtual ICollection<UserDruzyna> Druzynas { get; set; }

        public virtual ICollection<UserZastep> Zasteps { get; set; }

        [Display(Name = "Stopień harcerski")]
        public ScoutDegree ScoutDegree { get; set; }

        [Display(Name = "Stopień instruktorski")]
        public InstructorDegree InstructorDegree { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
        public virtual ICollection<FunctionInOrganization> FunctionInOrganizations { get; set; }
        public bool Recruitment { get; set; }
    }
}
