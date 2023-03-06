using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using RaceDataResult = DataOpResult<Race>;

    public class EfcRacesService : IRacesService
    {
        private readonly TypeRunnerContext _context;
        private readonly IUsersService _usersService;
        private readonly IUserRaceRecordsService _recordsService;
        private readonly IPassagesService _passagesService;

        public EfcRacesService(TypeRunnerContext context, IUsersService usersService, IUserRaceRecordsService recordsService, IPassagesService passagesService)
        {
            _context = context;
            _usersService = usersService;
            _recordsService = recordsService;
            _passagesService = passagesService;
        }

        public RaceDataResult AddUserToRaceWithMatchingWpmById(int userId)
        {
            var user = _usersService.GetById(userId);
            if (user == null)
                return RaceDataResult.Err(DataOpError.UserNotExist);

            var race = GetWaitingRaceWithAvgWpm(user.AverageWpm);
            if (race == null)
            {
                var createRaceResult = CreateRaceWithStartingAvgWpm(user.AverageWpm);
                if (!createRaceResult.Success)
                    return RaceDataResult.Err(DataOpError.NoMatchingRace | DataOpError.CannotCreateRace);
                race = createRaceResult.Result;
            }
            race.Racers.Add(user);
            _recordsService.CreateById(user.Id, race.Id);
            _context.SaveChanges();
            return RaceDataResult.Ok(race);
        }

        public RaceDataResult CreateRaceWithStartingAvgWpm(int wpm)
        {
            if (!_passagesService.HasPassage())
                return RaceDataResult.Err(DataOpError.NoPassagesFound);

            var race = new Race
            {
                AverageWpm = wpm,
                Passage = _passagesService.GetRandom()!,
            };
            _context.Races.Add(race);
            _context.SaveChanges();
            return RaceDataResult.Ok(race);
        }

        public RaceDataResult DeleteById(int id)
        {
            var race = _context.Races.Find(id);
            if (race == null)
                return RaceDataResult.Err(DataOpError.IdNotExist);
            _context.Races.Remove(race);
            _context.SaveChanges();
            return RaceDataResult.Ok();
        }

        public Race? GetById(int id)
        {
            return _context.Races.Find(id);
        }

        public Race? GetWaitingRaceWithAvgWpm(int wpm)
        {
            return _context.Races.FirstOrDefault(r => r.StartTime == null && IRacesService.WpmInRangeOfRaceAvgWpm(wpm, r));
        }

        public RaceDataResult RemoveUserFromRaceById(int raceId, int userId)
        {
            var user = _usersService.GetById(userId);
            if (user == null)
                return RaceDataResult.Err(DataOpError.UserNotExist);

            var race = GetById(raceId);
            if (race == null)
                return RaceDataResult.Err(DataOpError.RaceNotExist);

            race.Racers.Remove(user);
            _recordsService.DeleteById(user.Id, race.Id);
            _context.SaveChanges();
            return RaceDataResult.Ok(race);
        }
    }
}
