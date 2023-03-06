using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using RaceRecordDataResult = DataOpResult<UserRaceRecord>;
    public interface IUserRaceRecordsService
    {
        UserRaceRecord? GetById(int userId, int raceId);
        IList<UserRaceRecord> GetByUserId(int userId);
        IList<UserRaceMistake> GetMistakesByUserId(int userId);
        IList<(string Word, int NumberOfTimes)> GetMistakesWithNumOfTimesByUserId(int userId);
        RaceRecordDataResult CreateById(int userId, int raceId);
        RaceRecordDataResult UpdateWpm(int userId, int raceId, int wpm);
        RaceRecordDataResult DeleteById(int userId, int raceId);
        RaceRecordDataResult AddMistakeById(int userId, int raceId, string word);
    }
}
