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
        [Display(Name = "Przyboczny(-na)")] Przyboczny, [Display(Name = "Drużynowy(-wa)")] Druzynowy, Kapelan, Skarbnik,
        Komendant, [Display(Name = "Zastępca(-czyni) komendanta")] ZastepcaKomendanta, [Display(Name = "Członek")] Czlonek,
        [Display(Name = "Członek Komisji Rewizyjnej")] CzlonekKomisjiRewizyjnej, 
        [Display(Name = "Zastępca(-czyni) przewodniczącego Komisji Rewizyjnej")] ZastepcaPrzewodniczacegoKomisjiRewizyjnej, 
        [Display(Name = "Przewodniczący(-ca) Komisji Rewizyjnej")] PrzewodniczacyKomisjiRewizyjnej,
        [Display(Name = "Członek Sądu Harcerskiego")] CzlonekSaduHarcerskiego,
        [Display(Name = "Zastępca Przewodniczącego Sądu Harcerskiego")] ZastepcaPrzewodniczacegoSaduHarcerskiego,
        [Display(Name = "Przewodniczący(-ca) Sądu Harcerskiego")] PrzewodniczacySaduHarcerskiego,
        [Display(Name = "Naczelnik/Naczelniczka ZHP")] NaczelnikZHP,
        [Display(Name = "Zastępca(-czyni) Naczelnika ZHP")] ZastepcaNaczelnikaZHP
    }

    public class FunctionInOrganization
    {
        [Display(Name = "Indeks funkcji w organizacji")]
        public int FunctionInOrganizationId { get; set; }

        [Display(Name = "Indeks harcerza")]
        public string ScoutId { get; set; }

        [Display(Name = "Funkcja")]
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
