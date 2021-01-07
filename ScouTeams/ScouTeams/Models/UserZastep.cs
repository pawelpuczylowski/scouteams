using ScouTeams.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class UserZastep
    {
	    public string ScoutId { get; set; }
        public Scout MyScout { get; set; }
        public int ZastepId { get; set; }
        public Zastep Zastep { get; set; }
    }
}
