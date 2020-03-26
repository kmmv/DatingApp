using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers( DataContext context)
        {

            // km Check DB if there are no users in our database
            if(!context.Users.Any())
            {

                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);

                }

                context.SaveChanges();
                
            }

        }


        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // km :HMACSHA512 class with a random generated key
            // using keyword below so that the SHA class is diposed after this usage bloclk
          
            using( var hmac =  new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                // ComputeHash takes byte array - see the function to return a string to BYTE array
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        
    }
}