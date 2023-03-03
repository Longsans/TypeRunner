using Microsoft.EntityFrameworkCore;
using TypeRunnerBE.Models;

namespace TypeRunnerBE.Services.Data
{
    using UserCollectionDataResult = DataOpResult<IList<User>, UserDataOpError>;
    using UserDataResult = DataOpResult<User, UserDataOpError>;

    public class EfcUsersService : IUsersService
    {
        private readonly TypeMarathonContext _context;
        public EfcUsersService(TypeMarathonContext context)
        {
            _context = context;
        }

        // read
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public User? GetByCredentials(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public bool CheckUsernameAvailable(string username)
        {
            return !_context.Users.Any(x => x.Username == username);
        }

        // create, update, delete
        public UserDataResult Create(User user)
        {
            if (!CheckUsernameAvailable(user.Username))
                return UserDataResult.Err(UserDataOpError.UsernameTaken);

            _context.Users.Add(user);
            _context.SaveChanges();
            return UserDataResult.Ok(GetByUsername(user.Username));
        }

        public UserDataResult UpdateUsername(int id, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return UserDataResult.Err(UserDataOpError.InfoEmpty);

            if (!CheckUsernameAvailable(username))
                return UserDataResult.Err(UserDataOpError.UsernameTaken);

            var user = GetById(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            user.Username = username;
            _context.SaveChanges();
            return UserDataResult.Ok(user);
        }

        public UserDataResult UpdatePassword(int id, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return UserDataResult.Err(UserDataOpError.InfoEmpty);

            var user = GetById(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            user.Password = password;
            _context.SaveChanges();
            return UserDataResult.Ok(user);
        }

        public UserDataResult DeleteById(int id)
        {
            var user = GetById(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            _context.Remove(user);
            _context.SaveChanges();
            return UserDataResult.Ok(user);
        }

        // friends
        public UserCollectionDataResult GetFriendsById(int id)
        {
            var user = GetWithFriendEntities(id);
            if (user == null)
                return UserCollectionDataResult.Err(UserDataOpError.IdNotExist);

            var friends = GetFriends(user).ToList();
            return UserCollectionDataResult.Ok(friends);
        }

        public UserDataResult GetFriendByIdAndFriendId(int id, int friendId)
        {
            var user = GetWithFriendEntities(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            var friend = GetFriends(user).FirstOrDefault(x => x.Id == friendId);
            return UserDataResult.Ok(friend);
        }

        public UserDataResult GetFriendByIdAndFriendUsername(int id, string friendUsername)
        {
            var user = GetWithFriendEntities(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            var friend = GetFriends(user).FirstOrDefault(x => x.Username == friendUsername);
            return UserDataResult.Ok(friend);
        }

        public UserDataResult AddFriendByIdAndFriendId(int id, int friendId)
        {
            var user = GetWithFriendEntities(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);
            if (id == friendId)
                return UserDataResult.Err(UserDataOpError.FriendIsSelf);

            var friend = GetFriends(user).FirstOrDefault(f => f.Id == friendId);
            if (friend != null)
                return UserDataResult.Err(UserDataOpError.FriendAlreadyAdded);

            friend = GetById(friendId);
            if (friend == null)
                return UserDataResult.Err(UserDataOpError.FriendIdNotExist);

            user.FriendsTo.Add(friend);
            _context.SaveChanges();
            return UserDataResult.Ok(user);
        }

        public UserDataResult RemoveFriendByIdAndFriendId(int id, int friendId)
        {
            var user = GetWithFriendEntities(id);
            if (user == null)
                return UserDataResult.Err(UserDataOpError.IdNotExist);

            var friend = user.FriendsFrom.FirstOrDefault(f => f.Id == friendId);
            if (friend != null)
                user.FriendsFrom.Remove(friend);
            else
            {
                friend = user.FriendsTo.FirstOrDefault(f => f.Id == friendId);
                if (friend != null)
                    user.FriendsTo.Remove(friend);
                else return UserDataResult.Err(UserDataOpError.FriendNotAdded);
            }

            _context.SaveChanges();
            return UserDataResult.Ok(user);
        }

        private User? GetWithFriendEntities(int id)
        {
            return _context.Users.Include(u => u.FriendsFrom).Include(f => f.FriendsTo).FirstOrDefault(u => u.Id == id);
        }

        private static IEnumerable<User> GetFriends(User user)
        {
            return user.FriendsFrom.Concat(user.FriendsTo);
        }
    }
}
