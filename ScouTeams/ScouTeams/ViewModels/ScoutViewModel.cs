using Microsoft.AspNetCore.Mvc.Rendering;
using ScouTeams.Areas.Identity.Data;
using ScouTeams.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public class ScoutViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Funkcja")]
        public string Functions { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data urodzenia")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Opłacone składki")]
        public bool PaidContributions { get; set; }

        [Display(Name = "Nazwa funkcji")]
        public FunctionName functionName { get; set; }
        public int ThisOrganizationId { get; set; }
        public TypeOrganization ThisTypeOrganization { get; set; }


        [Display(Name = "Stopień harcerski")]
        public ScoutDegree ScoutDegree { get; set; }

        [Display(Name = "Stopień instruktorski")]
        public InstructorDegree InstructorDegree { get; set; }

    }
}
