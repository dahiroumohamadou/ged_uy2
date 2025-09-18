using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Liste
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? DateSign { get; set; }
        public string? Origine { get; set; }
        //public string? Type { get; set; }
        public string? Descritpion { get; set; }
        public string? NumeroCne { get; set; }
        public string? DateCne { get; set; }
        public int? Status { get; set; } = 0;
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        [NotMapped]
        public string? Years
        {
            get
            {
                return DateSign == null ? "" : this.DateSign.Substring(6);
            }
        }
    }
}
