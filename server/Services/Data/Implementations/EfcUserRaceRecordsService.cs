using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using RaceRecordDataResult = DataOpResult<UserRaceRecord>;

    public class EfcUserRaceRecordsService : IUserRaceRecordsService
    {
        private readonly TypeRunnerContext _context;
        private readonly IRacesService _racesService;
        private readonly IUsersService _usersService;

        public EfcUserRaceRecordsService(TypeRunnerContext context, IRacesService racesService, IUsersService usersService)
        {
            _context = context;
            _racesService = racesService;
            _usersService = usersService;
        }

        public RaceRecordDataResult AddMistakeById(int userId, int raceId, string word)
        {
            var record = _context.UserRaceRecords.Find(userId, raceId);
            if (record == null)
                return RaceRecordDataResult.Err(DataOpError.RecordNotExist);
            var mistake = record.Mistakes.FirstOrDefault(m => m.Word == word);
            if (mistake == null)
            {
                mistake = new UserRaceMistake
                {
                    UserId = userId,
                    RaceId = raceId,
                    Word = word,
                    NumberOfTimes = 1,
                };
                record.Mistakes.Add(mistake);
            }
            else
            {
                mistake.NumberOfTimes++;
            }
            _context.SaveChanges();
            return RaceRecordDataResult.Ok(record);
        }

        public RaceRecordDataResult CreateById(int userId, int raceId)
        {
            var user = _usersService.GetById(userId);
            if (user == null)
                return RaceRecordDataResult.Err(DataOpError.UserNotExist);

            var race = _racesService.GetById(raceId);
            if (race == null)
                return RaceRecordDataResult.Err(DataOpError.RaceNotExist);

            var record = new UserRaceRecord
            {
                User = user,
                Race = race,
                Wpm = 0,
            };
            _context.UserRaceRecords.Add(record);
            _context.SaveChanges();
            return RaceRecordDataResult.Ok(record);
        }

        public RaceRecordDataResult DeleteById(int userId, int raceId)
        {
            var record = GetById(userId, raceId);
            if (record == null)
                return RaceRecordDataResult.Err(DataOpError.RecordNotExist);
            _context.UserRaceRecords.Remove(record);
            _context.SaveChanges();
            return RaceRecordDataResult.Ok();
        }

        public UserRaceRecord? GetById(int userId, int raceId)
        {
            return _context.UserRaceRecords.Find(userId, raceId);
        }

        public IList<UserRaceRecord> GetByUserId(int userId)
        {
            return _context.UserRaceRecords.Where(rec => rec.UserId == userId).ToList();
        }

        public IList<UserRaceMistake> GetMistakesByUserId(int userId)
        {
            return _context.UserRaceRecords
                .Where(rec => rec.UserId == userId)
                .Select(rec => rec.Mistakes)
                .SelectMany(m => m)
                .ToList();
        }

        public IList<(string Word, int NumberOfTimes)> GetMistakesWithNumOfTimesByUserId(int userId)
        {
            return _context.UserRaceMistakes
                .Where(m => m.UserId == userId)
                .GroupBy(
                    m => m.Word,
                    m => m.NumberOfTimes,
                    (word, numOfTimes) => new Tuple<string, int>
                    (
                        word,
                        numOfTimes.Sum()
                    ).ToValueTuple())
                .ToList();
        }

        public RaceRecordDataResult UpdateWpm(int userId, int raceId, int wpm)
        {
            var record = _context.UserRaceRecords.Find(userId, raceId);
            if (record == null)
                return RaceRecordDataResult.Err(DataOpError.RecordNotExist);
            record.Wpm = wpm;
            _context.SaveChanges();
            return RaceRecordDataResult.Ok(record);
        }
    }
}
