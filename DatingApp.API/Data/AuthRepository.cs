using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext  _context;
        public AuthRepository(DataContext context)
        {
            _context = context;

        }

 

        public async Task<User> Login(string username, string password)
        {
             var user = await _context.Users.FirstOrDefaultAsync(x=>x.Username == username);
             if(user==null) return null; 

             if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null ;
            
            return user;
        }


        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            //get Salt and passwordHash
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x=>x.Username == username)) return true;
            return false; 
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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



        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //Verify previous password - See the passing of passwordSalt which is generated from the first function
              using( var hmac =  new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                
                // km computeHash is a byte array
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if(computeHash[i] !=  passwordHash[i]) return false;
                }

            }

            // If there was a mismatch if would have return false. This point the array is correctly mathching to the passwordHash
            return true;
        }
    }
}