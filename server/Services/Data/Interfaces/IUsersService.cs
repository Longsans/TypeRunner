using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using UserDataResult = DataOpResult<User>;
    using UserListDataResult = DataOpResult<IList<User>>;

    public interface IUsersService
    {
        // read
        IQueryable<User> GetAll();
        User? GetById(int id);
        User? GetByUsername(string username);
        User? GetByCredentials(string username, string password);
        bool CheckUsernameAvailable(string username);
        // create, update, delete
        UserDataResult Create(User user);
        UserDataResult UpdateUsername(int id, string username);
        UserDataResult UpdatePassword(int id, string password);
        UserDataResult DeleteById(int id);
        // friends
        UserListDataResult GetFriendsById(int id);
        UserDataResult GetFriendByIdAndFriendId(int id, int friendId);
        UserDataResult GetFriendByIdAndFriendUsername(int id, string friendUsername);
        UserDataResult AddFriendByIdAndFriendId(int id, int friendId);
        UserDataResult RemoveFriendByIdAndFriendId(int id, int friendId);
    }
}
