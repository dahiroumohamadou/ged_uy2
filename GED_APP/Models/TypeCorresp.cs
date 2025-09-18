using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class TypeCorresp
    {
        public int Id { get; set; }
        public string? Libele { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Correspondance>? Correspondances { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
}
