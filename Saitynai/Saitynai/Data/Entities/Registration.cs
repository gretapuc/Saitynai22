using Saitynai.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace Saitynai.Data.Entities
{
    public class Registration : IUserOwnedResource
    {
        public int Id { get; set; }
        public string CarNo { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }

        public Competition Competition { get; set; }

        [Required]
        public string UserId { get; set; }

    }
}
