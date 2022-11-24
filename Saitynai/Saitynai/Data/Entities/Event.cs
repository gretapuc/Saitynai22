using Saitynai.Auth.Model;
using System.ComponentModel.DataAnnotations;

namespace Saitynai.Data.Entities;

    public class Event : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        public IsmRestUser User { get; set; }
    }

