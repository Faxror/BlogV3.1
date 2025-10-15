using System.ComponentModel.DataAnnotations;

namespace BlogV3._1.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "İsim zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Telefon Numarası zorunludur")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }

        public string ProfilImage { get; set; }


    }
}
