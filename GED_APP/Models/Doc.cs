using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Doc
    {
        public int Id { get; set; }
        public string? TypeDoc { get; set; }
        public string? Objet { get; set; }
        public string? Source { get; set; }
        public string? Numero { get; set; }
        public string? DateSign { get; set; }
        public string? Signataire { get; set; }
        public string? AnneeAcademique { get; set; }
        public int? CycleId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Cycle? Cycle { get; set; }
        public int? Fichier { get; set; }
        public string? Session { get; set; }
        public string? Promotion { get; set; }
        public string? AnneeSortie { get; set; }
        public int? FiliereId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Filiere? Filiere { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        [NotMapped]
        public string? FiliereInfo
        {
            get
            {
                return Filiere == null ? "" : this.Filiere.Code;
            }
        }
        [NotMapped]
        public string? CycleInfo
        {
            get
            {
                return Cycle == null ? "" : this.Cycle.Code;
            }
        }
        [NotMapped]
        public string? CodePvAnnee
        {
            get
            {
                return Filiere == null ? "" : "PV/"+this.Filiere.Code+"/"+this.AnneeSortie;
            }
        }
        [NotMapped]
        public string? CodeDocArr
        {
            get
            {
                return Cycle == null ? "" : "ARR/" + this.Cycle.Code + "/" + this.AnneeAcademique;
            }
        }
        [NotMapped]
        public string? CodeDocCrp
        {
            get
            {
                return Cycle == null ? "" : "CRP/" + this.Cycle.Code + "/" + this.AnneeAcademique;
            }
        }
    }
}
