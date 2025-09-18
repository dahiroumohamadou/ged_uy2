using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Correspondance
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Date { get; set; }
        public string? Objet {  get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
