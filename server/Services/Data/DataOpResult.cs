namespace TypeRunnerBE.Services.Data
{
    public class DataOpResult<TResult>
        where TResult : class
    {
        public bool Success;
        public TResult? Result;
        public DataOpError? Error;

        public static DataOpResult<TResult> Ok(TResult? result = null)
        {
            return new DataOpResult<TResult>
            {
                Success = true,
                Result = result,
                Error = null
            };
        }

        public static DataOpResult<TResult> Err(DataOpError error)
        {
            return new DataOpResult<TResult>
            {
                Success = false,
                Result = null,
                Error = error
            };
        }
    }

    public enum DataOpError
    {
        None = 0,
        IdNotExist,

        // Users
        UserNotExist,
        UsernameTaken,
        InfoEmpty,
        FriendIdNotExist,
        FriendAlreadyAdded,
        FriendNotAdded,
        FriendIsSelf,

        // Races
        RaceNotExist,
        NoMatchingRace,
        CannotCreateRace,

        // Race records
        RecordNotExist,
        RecordAlreadyExists,

        // Passage
        PassageAlreadyExists,
        NoPassagesFound,

        // Source
        SourceNotExist,
        SourceAlreadyExists
    }
}
