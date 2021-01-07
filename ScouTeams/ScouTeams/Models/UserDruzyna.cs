using System;
using ScouTeams.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public class UserDruzyna
    {
	    public string ScoutId { get; set; }
        public Scout MyScout { get; set; }
        public int DruzynaId { get; set; }
        public Druzyna Druzyna { get; set; }
    }
}
