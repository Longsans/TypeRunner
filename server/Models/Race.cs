using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class Race
    {
        [Key]
        public int Id { get; set; }

        public ICollection<User> Racers { get; set; }

        [Required]
        public ICollection<UserRaceRecord> UserRecords { get; set; }

        [Required]
        public int AverageWpm { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required]
        public Passage Passage { get; set; }
        public int PassageId { get; set; }
    }
}
