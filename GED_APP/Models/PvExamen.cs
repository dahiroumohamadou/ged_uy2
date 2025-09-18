using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class PvExamen
    {
        public int Id { get; set; }
        public string? Source { get; set; }
        public int? FiliereId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Filieree? Filiere { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
