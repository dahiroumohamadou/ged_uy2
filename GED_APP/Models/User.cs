namespace GED_APP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? SaltPassword { get; set; }
        public string? Token { get; set; }
        public bool KeepLoginIn { get; set; }
    }
}
