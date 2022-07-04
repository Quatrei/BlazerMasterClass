using System.ComponentModel;

namespace CovacRegistration.Shared.Enums
{
    public enum VaccinationVenue
    {
        [Description("Araneta Coliseum")]
        AranetaColiseum = 1,

        [Description("Ateneo Grade School")]
        AteneoGradeSchool,

        [Description("Ayala Malls Trinoma")]
        Trinoma,

        [Description("QC Hall Covered Walk")]
        QCHall,

        [Description("Robinsons Magnolia")]
        RobinsonsMagnolia,

        [Description("SM Fairview")]
        SMFairview,

        [Description("SM North Skydome")]
        SMSkydome
    }
}
