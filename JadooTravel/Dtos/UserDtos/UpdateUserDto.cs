using System.ComponentModel.DataAnnotations;

namespace JadooTravel.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        [Required]
        public string UserId { get; set; }  

        [Required(ErrorMessage = "Ad alanı boş bırakılamaz!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        public string UserName { get; set; }

        public string? Password { get; set; }

        public string? ImageUrl { get; set; }
    }
}

