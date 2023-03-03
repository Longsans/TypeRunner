using TypeRunnerBE.Models;

namespace TypeRunnerBE.Utilities
{
    public static class DbInitializer
    {
        public static async Task Initialize(TypeMarathonContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var user1 = new User
            {
                Id = 1,
                Username = "asdf",
                Password = "asdf",
                FriendsFrom = new List<User>(),
                FriendsTo = new List<User>(),
            };
            var user2 = new User
            {
                Id = 2,
                Username = "qwer",
                Password = "qwer",
                FriendsFrom = new List<User>(),
                FriendsTo = new List<User>(),
            };
            var user3 = new User
            {
                Id = 3,
                Username = "laskdjflkajsdf",
                Password = "laskdjflaksjdf",
                FriendsFrom = new List<User>(),
                FriendsTo = new List<User>(),
            };
            context.Users.AddRange(user1, user2, user3);
            user1.FriendsFrom.Add(user3);
            user1.FriendsTo.Add(user2);
            user2.FriendsFrom.Add(user1);
            user2.FriendsFrom.Add(user3);
            user3.FriendsTo.Add(user1);
            user3.FriendsTo.Add(user2);
            await context.SaveChangesAsync();
        }
    }
}
