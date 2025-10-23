using System.ComponentModel.DataAnnotations;

namespace JadooTravel.Dtos.UserDtos
{
    public class UpdateUserPasswordDto
    {
        [Required(ErrorMessage = "Kullanıcı ID gerekli!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Mevcut şifreyi giriniz!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifreyi giriniz!")]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır!")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor!")]
        public string ConfirmNewPassword { get; set; }
    }
}
