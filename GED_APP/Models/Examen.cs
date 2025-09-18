using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Examen
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Session { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<PvExamen>? Pvs { get; set; }

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
