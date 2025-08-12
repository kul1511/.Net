using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Data;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Services;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Repository
{
    public class RegisterRepository : IAuth
    {
        private readonly CropDealDbContext c;
        private readonly PasswordService _passwordService;
        private readonly ILogger<RegisterRepository> log;
        public RegisterRepository(CropDealDbContext _r, ILogger<RegisterRepository> _log, PasswordService ps)
        {
            c = _r;
            log = _log;
            _passwordService = ps;
        }

        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await c.Logs.AddAsync(log);
            await c.SaveChangesAsync();
        }

        public async Task<User?> Login(LoginRequest request)
        {
            var user = await c.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return null;
            }

            var passwordService = new PasswordService();
            if (!passwordService.VerifyPassword(user.Password_Hash, request.Password_Hash))
            {
                return null;
            }
            return user;
        }

        public async Task<bool> Register(User u)
        {
            await LogToDatabase("Info", $"Registration attempt for User: {u.FullName}", DateTime.Now);

            try
            {

                if (u.Role != UserRole.Farmer && u.Role != UserRole.Dealer && u.Role != UserRole.Admin)
                {
                    await LogToDatabase("Failed", "Role should 0 or 1 or 2", DateTime.Now);
                    return false;
                }

                if (await c.Users.AnyAsync(user => user.Email == u.Email) || await c.Users.AnyAsync(user => user.UserId == u.UserId))
                {
                    await LogToDatabase("Failed", $"User Already Exists for User: {u.FullName} and Email: {u.Email}, UserId: {u.UserId}", DateTime.Now);
                    return false;
                }
                var passwordService = new PasswordService();
                u.Password_Hash = passwordService.HashPassword(u.Password_Hash);
                c.Users.Add(u);
                await c.SaveChangesAsync();

                await LogToDatabase("Success", $"Successfull Registration for User: {u.FullName} with Id: {u.UserId}", DateTime.Now);
            }
            catch (Exception e)
            {
                await LogToDatabase("Failed", $"Registration failed for User: {u.FullName}. Exception : {e.Message}", DateTime.Now);
            }

            return true;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await c.Users.ToListAsync();
            return users;
        }

        // Get user by Id
        public async Task<User?> GetUserById(string userId)
        {
            var user = await c.Users.FindAsync(userId);
            return user == null ? null : user;
        }
        public async Task<bool> UpdateUser(string userId, User updatedUser)
        {
            var user = await c.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Update profile details
            user.FullName = updatedUser.FullName ?? user.FullName;
            user.Email = updatedUser.Email ?? user.Email;

            // Hash password if provided
            if (!string.IsNullOrEmpty(updatedUser.Password_Hash))
            {
                user.Password_Hash = _passwordService.HashPassword(updatedUser.Password_Hash);
            }

            user.Status = updatedUser.Status ?? user.Status;
            c.Users.Update(user);
            await c.SaveChangesAsync();
            await LogToDatabase("Success", $"User {user.UserId} updated successfully.", DateTime.Now);
            return true;
        }
        public async Task<bool> DeleteUser(string userId)
        {
            var user = await c.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Hard delete - Remove from the database
            c.Users.Remove(user);
            await c.SaveChangesAsync();
            await LogToDatabase("Success", $"User {user.UserId} deleted successfully from the database.", DateTime.Now);
            return true;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await c.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}