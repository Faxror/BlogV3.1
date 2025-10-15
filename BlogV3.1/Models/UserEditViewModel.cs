using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogV3._1.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public string ProfilImageUrl { get; set; }
        public List<string>? UserRoles { get; set; }
        public List<SelectListItem>? AllRoles { get; set; }
        public string? SelectedRole { get; set; }


    }
}
