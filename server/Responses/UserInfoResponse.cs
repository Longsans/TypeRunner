using TypeRunnerBE.Models;

namespace TypeRunnerBE.Responses
{
    public class UserInfoResponse
    {
        public int Id { get; }
        public string Username { get; }

        public UserInfoResponse(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}
