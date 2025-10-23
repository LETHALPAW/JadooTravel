using System.ComponentModel.DataAnnotations;

namespace JadooTravel.Dtos.UserDtos
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır!")]
        public string Password { get; set; }

        public string? ImageUrl { get; set; }
    }
}
