using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class PvCNE
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? CNEId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public CNE? CNE { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
