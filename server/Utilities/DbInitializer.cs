using TypeRunnerBE.Models;

namespace TypeRunnerBE.Utilities
{
    public static class DbInitializer
    {
        public static async Task Initialize(TypeRunnerContext context)
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

            var source1 = new Source
            {
                Name = "Cormac McCarthy, No Country for Old Men ",
            };
            context.Sources.Add(source1);
            var p1 = new Passage
            {
                Source = source1,
                Text = "You think when you wake up in the mornin yesterday don't count. But yesterday is all that does count. What else is there? Your life is made out of the days it’s made out of. Nothin else.",
            };
            var p2 = new Passage
            {
                Source = source1,
                Text = "I think by the time you're grown you're as happy as you're goin to be. You'll have good times and bad times, but in the end you'll be about as happy as you was before. Or as unhappy. I've knowed people that just never did get the hang of it."
            };
            var p3 = new Passage
            {
                Source = source1,
                Text = "People complain about the bad things that happen to em that they don't deserve but they seldom mention the good. About what they done to deserve them things"
            };
            context.Passages.AddRange(p1, p2, p3);
            await context.SaveChangesAsync();
        }
    }
}
