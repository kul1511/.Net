using CROPDEAL.Models;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IBankAccount
    {
        Task<IEnumerable<BankAccount>> GetAllBankAccounts();
        Task<BankAccount> GetBankAccountByUser(int userId);
        Task<bool> AddBankAccount(BankAccountDTO bankAccount);
        // Task<bool> UpdateBankAccount(BankAccountDTO bankAccount);
        Task<bool> DeleteBankAccount(int bankAccountId);
    }
}