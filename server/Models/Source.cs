using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class Source
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ICollection<Passage> Passages { get; set; }
    }
}
