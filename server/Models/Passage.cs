using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class Passage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public Source Source { get; set; }
        public int SourceId { get; set; }
    }
}
