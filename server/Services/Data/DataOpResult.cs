namespace TypeRunnerBE.Services.Data
{
    public class DataOpResult<TResult, TError>
        where TResult : class
        where TError : Enum
    {
        public bool Success;
        public TResult? Result;
        public TError? Error;

        public static DataOpResult<TResult, TError> Ok(TResult? result)
        {
            return new DataOpResult<TResult, TError>
            {
                Success = true,
                Result = result,
                Error = default
            };
        }

        public static DataOpResult<TResult, TError> Err(TError error)
        {
            return new DataOpResult<TResult, TError>
            {
                Success = false,
                Result = null,
                Error = error
            };
        }
    }
}
