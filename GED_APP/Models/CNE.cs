using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class CNE
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Description { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? PvId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public PvCNE? Pv { get; set; }
        public int? ArreteId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Arrete? Arrete { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Liste>? Listes { get; set; }
    }
}
