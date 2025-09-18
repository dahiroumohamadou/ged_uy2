using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Dossier
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Origine { get; set; }
        public string? Demandeur { get; set; }
        public string? Objet { get; set; }
        public string? PersonneTraite { get; set; }
        public string? DateEntree { get; set; }
        public string? DateSortie { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? Status { get; set; } = 0;
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        [NotMapped]
        public string? Years
        {
            get
            {
                return DateSortie == null ? "" : this.DateSortie.Substring(6);
            }
        }
    }
}

