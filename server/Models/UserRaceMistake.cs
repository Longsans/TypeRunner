using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class UserRaceMistake
    {
        public UserRaceRecord UserRace { get; set; }
        public int UserId { get; set; }
        public int RaceId { get; set; }

        [Required]
        public string Word { get; set; }
    }
}
