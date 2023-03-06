using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using RaceDataResult = DataOpResult<Race>;

    public interface IRacesService
    {
        Race? GetById(int id);
        Race? GetWaitingRaceWithAvgWpm(int wpm);
        RaceDataResult CreateRaceWithStartingAvgWpm(int wpm);
        RaceDataResult AddUserToRaceWithMatchingWpmById(int userId);
        RaceDataResult RemoveUserFromRaceById(int raceId, int userId);
        RaceDataResult DeleteById(int id);
        static bool WpmInRangeOfRaceAvgWpm(int wpm, Race race)
        {
            return race.AverageWpm - 10 <= wpm && wpm <= race.AverageWpm + 10;
        }
    }
}
