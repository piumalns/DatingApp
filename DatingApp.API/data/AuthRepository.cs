using System;
using System.Threading.Tasks;
using DatingApp.API.data.Interface;
using DatingApp.API.model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context { get; }

        public AuthRepository(DataContext context)
        {
            _context = context;
        }        

        public async Task<User> Login(string Username, string Password)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == Username);

            if(user == null){
                return null;
            }

            if(!VerifyPasswordHash(Password,user.PasswordHash,user.PasswordSalt)){
                return null;
            }

            return user;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmc = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i =0; i < ComputeHash.Length; i++){
                    if(ComputeHash[i] != passwordHash[i])
                    return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string Password)
        {
            byte[] PasswordHash, PasswordSalt;
            CreatePasswordHash(Password,out PasswordHash,out PasswordSalt);

            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;

            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmc = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmc.Key;
                passwordHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExits(string Username)
        {
            if(await _context.users.AnyAsync(x => x.Username == Username))
                return false;

            return true;
        }
    }
}