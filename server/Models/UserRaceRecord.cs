using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class UserRaceRecord
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public Race Race { get; set; }
        public int RaceId { get; set; }

        [Required]
        public int Wpm { get; set; }

        public ICollection<UserRaceMistake> Mistakes { get; set; }
    }
}
