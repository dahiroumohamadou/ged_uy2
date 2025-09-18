using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Arrete
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Objet { get; set; }
        public string? DateSign { get; set; }
        public int? CNEId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public CNE? CNE { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
