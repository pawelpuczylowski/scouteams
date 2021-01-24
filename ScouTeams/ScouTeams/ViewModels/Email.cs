using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.ViewModels
{
    public class Email
    {
        public int OrganizationID { get; set; }

        [Display(Name = "Organizacja")]
        public TypeOrganization type { get; set; }

        [Display(Name = "Email")]
        public string ScoutEmail { get; set; }

        [Display(Name = "Temat")]
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Treść")]
        public string Message { get; set; }
    }
}
