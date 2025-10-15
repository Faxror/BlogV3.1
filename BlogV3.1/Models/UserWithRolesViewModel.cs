namespace BlogV3._1.Models
{
    public class UserWithRolesViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? ProfilImage { get; set; }
        public List<string>? Roles { get; set; }
    }
}
