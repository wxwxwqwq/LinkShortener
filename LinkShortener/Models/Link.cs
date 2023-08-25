using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Models
{
    public class Link
    {
        public static List<Link> AllLinks = new List<Link>();
        public static List<Link> UserLinks = new List<Link>();       

        public static Userr Authorized = null;

        [ValidateNever]
        public int id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести ссылку")]
        public string url { get; set; }

        [ValidateNever]
        public string shorturl { get; set; }

        [ValidateNever]
        public string token { get; set; }

        [ValidateNever]
        public DateTime dateOfCreation { get; set; }

        [ValidateNever]
        public int clicks { get; set; }

        [ValidateNever]
        public string creator { get; set; }
    }
}
