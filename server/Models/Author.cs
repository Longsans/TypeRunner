using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<Quote> Quotes { get; set; }
    }
}
