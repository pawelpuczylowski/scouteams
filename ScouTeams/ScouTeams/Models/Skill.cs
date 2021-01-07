using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public enum Difficulty
    {
        I, II, III, M
    }
    public class Skill
    {
        [Display(Name = "Indeks sprawności")]
        public int SkillId { get; set; }

        [Display(Name = "Stopień trudności")]
        public Difficulty Difficulty { get; set; }

        [Display(Name = "Indeks harcerza")]
        public string ScoutId { get; set; }

        [Display(Name = "Nazwa sprawności")]
        public string Name { get; set; }
    }
}
