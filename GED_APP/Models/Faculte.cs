namespace GED_APP.Models
{
    public class Faculte
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Libele { get; set; }
        public ICollection<PvExamen>? PvExamens { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
