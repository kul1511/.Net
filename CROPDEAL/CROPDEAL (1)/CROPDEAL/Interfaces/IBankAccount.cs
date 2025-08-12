using CROPDEAL.Models;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IBankAccount
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<BankAccount>> GetAllBankAccounts();
        Task<BankAccount> GetBankAccountByUser(string userId);
        Task<bool> AddBankAccount(BankAccountDTO bankAccount);
        // Task<bool> UpdateBankAccount(BankAccountDTO bankAccount);
        Task<bool> DeleteBankAccount(string bankAccountId);
    }
}