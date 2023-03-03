using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
