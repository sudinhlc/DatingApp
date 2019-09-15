using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passworSalt;
                CreatePasswordHash("password", out passwordHash, out passworSalt);
                user.PasswordHash=passwordHash;
                user.PasswordSalt=passworSalt;
                user.Username=user.Username.ToLower();
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passworSalt)
        {
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                passworSalt=hmac.Key;
                passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }            
        }
    }
}