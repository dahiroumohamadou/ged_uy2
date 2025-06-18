using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Tracker
    {
        public int Id { get; set; }
        public int? DocumentId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public  Doc? Doc { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public User? User { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
