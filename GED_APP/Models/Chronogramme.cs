using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GED_APP.Models
{
    public class Chronogramme
    {
        public int Id { get; set; }
        public string? NumeroPTA { get; set; }
        public string? Libele { get; set; }
        public string? Annee { get; set; }
        public string? Origine { get; set; }
        public int? StructureId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Structure? Structure { get; set; }
        public int? Status { get; set; } = 0;
        
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;

            [NotMapped]
        public int? GetId
        {
            get
            {
                return Structure == null ? -1 : this.Structure.Id;
            }
        }
        //[NotMapped]
        //public string? Years
        //{
        //    get
        //    {
        //       // return Annee == null ? "" : this.Annee.Substring(6);
        //    }
        //}
    }
}
