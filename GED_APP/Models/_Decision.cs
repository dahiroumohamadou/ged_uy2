namespace GED_APP.Models
{
    public class _Decision
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public string? Objet { get; set; }
        public string? Type { get; set; }
        public string? Signataire { get; set; }
        public string? DateSign { get; set; }
        public int? Status { get; set; } = 0;

        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
