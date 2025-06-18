using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Filiere
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Libele { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public ICollection<Doc>? Documents { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
