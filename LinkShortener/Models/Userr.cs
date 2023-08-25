using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Models
{
    public class Userr
    {
        public static List<Userr> AllUsers = new List<Userr>();

        [Required(ErrorMessage = "Необходимо ввести логин")]
        public string login { get; set; }

        [Required(ErrorMessage = "Необходимо ввести пароль")]
        public string password { get; set; }
    }
}
