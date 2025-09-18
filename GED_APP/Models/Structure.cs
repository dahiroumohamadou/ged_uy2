using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Structure
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Libele { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Chronogramme>? Chronos { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Correspondance>? Correspondances { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Dossier>? Dossiers { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<PvCNE>? PvCNEs { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<PvExamen>? PvExamens { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Rapport>? Rapports { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Arrete>? Arretes { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Liste>? Listes { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Decharge>? Decharges { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
