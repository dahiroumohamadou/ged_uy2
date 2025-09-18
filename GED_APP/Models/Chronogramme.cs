using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Chronogramme
    {
        public int Id { get; set; }
        public int? Numero { get; set; }
        public string? Libele { get; set; }
        public int? Annee { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? Status { get; set; } = 0;
        
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
