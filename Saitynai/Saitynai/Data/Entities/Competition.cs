using Saitynai.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace Saitynai.Data.Entities
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }

        public Event Event { get; set; }

        [Required]
        public string UserId { get; set; }

        public IsmRestUser User { get; set; }
    }
}
