using Midterm.Api.Models;
using Midterm.Data;


using Microsoft.EntityFrameworkCore;

using Midterm.Helpers;


namespace Midterm.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Users.AnyAsync())
            {
                return;
            }

            var users = new List<User>
            {
                new User
                {
                    FullName = "Demo Host",
                    Email = "host@test.com",
                    PasswordHash = PasswordHasher.Hash("123456"),
                    Role = "Host",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Demo Guest",
                    Email = "guest@test.com",
                    PasswordHash = PasswordHasher.Hash("123456"),
                    Role = "Guest",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    FullName = "Demo Admin",
                    Email = "admin@test.com",
                    PasswordHash = PasswordHasher.Hash("123456"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }
    }
}