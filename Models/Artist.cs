using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicReviewsWebsite.Models
{
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Also Known As")]
        public string AlsoKnownAs { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
