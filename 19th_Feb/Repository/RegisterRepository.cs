using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Data;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Services;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace CROPDEAL.Repository
{
    public class RegisterRepository : IAuth
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RegisterRepository));
        private readonly CropDealDbContext c;
        private readonly PasswordService _passwordService;
        public RegisterRepository(CropDealDbContext _r, PasswordService ps)
        {
            c = _r;
            _passwordService = ps;
        }

        public async Task<User> Login(LoginRequest request)
        {
            var user = await c.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.Warn($"Login failed: user not found for email {request.Email}");
                return null!;
            }

            var passwordService = new PasswordService();
            if (!passwordService.VerifyPassword(user.Password_Hash!, request.Password_Hash!))
            {
                _logger.Warn($"Login failed: password mismatch for user {request.Email}");
                return null!;
            }
            _logger.Info($"Login successful for user {request.Email}");
            return user;
        }


        public async Task<bool> Register(User u)
        {
            _logger.Info($"Registration attempt for User: {u.FullName}");
            try
            {

                if (u.Role != UserRole.Farmer && u.Role != UserRole.Dealer && u.Role != UserRole.Admin)
                {
                    _logger.Warn($"Role should 0 or 1 or 2");
                    return false;
                }

                if (await c.Users.AnyAsync(user => user.Email == u.Email) || await c.Users.AnyAsync(user => user.UserId == u.UserId))
                {
                    _logger.Warn($"User Already Exists for User: {u.FullName} and Email: {u.Email}, UserId: {u.UserId}");
                    return false;
                }
                var passwordService = new PasswordService();
                u.Password_Hash = passwordService.HashPassword(u.Password_Hash!);
                c.Users.Add(u);
                await c.SaveChangesAsync();

                _logger.Info($"Successfull Registration for User: {u.FullName} with Id: {u.UserId}");
            }
            catch (Exception e)
            {
                _logger.Error($"Registration failed for User: {u.FullName}. Exception : {e.Message}");
            }

            return true;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await c.Users.ToListAsync();
            _logger.Info($"Retrieved {users.Count} users.");
            return users;
        }

        // Get user by Id
        public async Task<User?> GetUserById(string userId)
        {
            var user = await c.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.Warn($"User not found for Id: {userId}");
                return null;
            }
            _logger.Info($"Retrieved user for Id: {userId}");
            return user;
        }
        public async Task<bool> UpdateUser(string userId, User updatedUser)
        {
            var user = await c.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.Warn($"Update failed: user not found for Id: {userId}");
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
            _logger.Info($"User {user.UserId} updated successfully.");
            return true;
        }
        public async Task<bool> DeleteUser(string userId)
        {
            var user = await c.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.Warn($"Delete failed: user not found for Id: {userId}");
                return false;
            }

            // Hard delete - Remove from the database
            c.Users.Remove(user);
            await c.SaveChangesAsync();
            _logger.Info($"User {user.UserId} deleted successfully from the database.");
            return true;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await c.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                _logger.Warn($"User not found for email: {email}");
            }
            else
            {
                _logger.Info($"Retrieved user for email: {email}");
            }
            return user;
        }
    }
}