using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public enum TypeOrganization
    {
        Zastep, Druzyna, Hufiec, Choragiew, KwateraGlowna
    }
    public class Assignment
    {
        [Display(Name = "Typ organizacji")]
        public TypeOrganization TypeOrganization { get; set; }

        [Display(Name = "Indeks organizacji")]
        public int OrganizationId { get; set; }

        [Display(Name = "Nazwa organizacji")]
        public string Name { get; set; }

        public Assignment() { }
        public Assignment(TypeOrganization TypeOrganization, int OrganizationId, string Name)
        {
            this.TypeOrganization = TypeOrganization;
            this.OrganizationId = OrganizationId;
            this.Name = Name;
        }
    }
}
