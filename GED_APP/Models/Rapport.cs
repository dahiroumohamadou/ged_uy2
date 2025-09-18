using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Rapport
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Periode { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
