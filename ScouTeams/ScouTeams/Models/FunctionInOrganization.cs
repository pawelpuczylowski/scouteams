using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScouTeams.Models
{
    public enum FunctionName
    {
        Brak, [Display(Name = "Podzastępowy(-wa)")] Podzastepowy, [Display(Name = "Zastępowy(-wa)")] Zastepowy, 
        Przyboczny, [Display(Name = "Drużynowy(-wa)")] Druzynowy, Kapelan,
        Komendant, [Display(Name = "Zastępca komendanta")] ZastępcaKomendanta, [Display(Name = "Członek")] Czlonek,
        [Display(Name = "Członek Komisji Rewizyjnej")] CzłonekKomisjiRewizyjnej, 
        [Display(Name = "Zastępca przewodniczącego Komisji Rewizyjnej")] ZastępcaPrzewodniczącegoKomisjiRewizyjnej, 
        [Display(Name = "Przewodniczący(-ca) Komisji Rewizyjnej")] PrzewodniczącyKomisjiRewizyjnej,
        [Display(Name = "Członek Sądu Harcerskiego")] CzłonekSąduHarcerskiego,
        [Display(Name = "Zastępca Przewodniczącego Sądu Harcerskiego")] ZastępcaPrzewodniczącegoSąduHarcerskiego,
        [Display(Name = "Przewodniczący(-ca) Sądu Harcerskiego")] PrzewodniczącySąduHarcerskiego
    }

    public class FunctionInOrganization
    {
        [Display(Name = "Indeks funkcji w organizacji")]
        public int FunctionInOrganizationId { get; set; }

        [Display(Name = "Indeks harcerza")]
        public string ScoutId { get; set; }

        [Display(Name = "Indeks użytkownika")]
        public FunctionName FunctionName { get; set; }

        [Display(Name = "Indeks chorągwi")]
        public int ChorągiewId { get; set; }

        [Display(Name = "Indeks hufca")]
        public int HufiecId { get; set; }

        [Display(Name = "Indeks drużyny")]
        public int DruzynaId { get; set; }

        [Display(Name = "Indeks zastępu")]
        public int ZastepId { get; set; }

    }
}
