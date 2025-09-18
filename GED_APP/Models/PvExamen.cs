using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class PvExamen
    {
        public int Id { get; set; }
        public string? Session { get; set; }
        public int? FiliereId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Filieree? Filiere { get; set; }
        public int? ExamenId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Examen? Examen { get; set; }
        public int? FaculteId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Faculte? Faculte { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? SourceId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Source? Source { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;

        [NotMapped]
        public string? ExamenInfo
        {
            get
            {
                return Examen == null ? "" : this.Examen.Code;
            }
        }
        [NotMapped]
        public string? FiliereInfo
        {
            get
            {
                return Filiere == null ? "" : this.Filiere.Code;
            }
        }
        [NotMapped]
        public string? FaculteInfo
        {
            get
            {
                return Faculte == null ? "" : this.Faculte.Code;
            }
        }
        [NotMapped]
        public string? SourceInfo
        {
            get
            {
                return Source == null ? "" : this.Source.Code;
            }
        }
        [NotMapped]
        public string? Years
        {
            get
            {
                return Session == null ? "" : this.Session.Substring(3);
            }
        }
    }
}
