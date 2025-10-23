using System.ComponentModel.DataAnnotations;

namespace JadooTravel.Dtos.UserDtos
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz!")]
        public string Password { get; set; }
    }
}

