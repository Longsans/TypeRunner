using System.ComponentModel.DataAnnotations;

namespace TypeRunnerBE.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public ICollection<User> FriendsFrom { get; set; }

        [Required]
        public ICollection<User> FriendsTo { get; set; }

        public ICollection<UserRaceRecord> RaceRecords { get; set; }

        public Race? CurrentRace { get; set; }
        public int? CurrentRaceId { get; set; }
    }
}
