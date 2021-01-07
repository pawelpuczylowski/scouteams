using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class UserChoragiew
    {
	    public string ScoutId { get; set; }
        public Scout MyScout { get; set; }
        public int ChoragiewId { get; set; }
        public Choragiew Choragiew { get; set; }
    }
}
