using CROPDEAL.Models;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IBankAccount
    {
        Task<IEnumerable<BankAccount>> GetAllBankAccounts();
        Task<BankAccountDTO> GetBankAccountByUser(string userEmail);
        Task<bool> AddBankAccount(BankAccountDTO bankAccount);
        Task<bool> DeleteBankAccount(string bankAccountId);
    }
}