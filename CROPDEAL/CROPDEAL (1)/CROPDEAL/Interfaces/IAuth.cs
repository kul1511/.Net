using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CROPDEAL.Interfaces
{
    public interface IAuth
    {
        Task<bool> Register(User user);
        Task<User> Login(LoginRequest request);
        Task<User> GetUserByEmailAsync(string email);
        Task LogToDatabase(string level, string message, DateTime now);
        Task<bool> DeleteUser(string id);
        Task<bool> UpdateUser(string id, User userDTO);
        Task<IEnumerable<User>> GetAllUsers();
    }
}