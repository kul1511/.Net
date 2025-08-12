using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CROPDEAL.Repository
{
    public class BankAccountRepository : IBankAccount
    {
        private readonly ILogger<BankAccountRepository> log;
        private readonly CropDealDbContext crop;
        private readonly IMapper mapper;
        public BankAccountRepository(CropDealDbContext user, IMapper _mapper, ILogger<BankAccountRepository> _log)
        {
            crop = user;
            mapper = _mapper;
            log = _log;
        }

        public async Task<bool> AddBankAccount(BankAccountDTO bankAccount)
        {
            log.LogInformation($"Trying to Add Bank Account: {bankAccount.AccountNumber}", DateTime.Now);

            var bankAcc = mapper.Map<BankAccount>(bankAccount);

            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccount.BankId);

            if (res == null)
            {
                await crop.BankAccounts.AddAsync(bankAcc);
                await crop.SaveChangesAsync();
                log.LogInformation($"Successfully Added Bank Account: {bankAccount.AccountNumber}", DateTime.Now);
                return true;
            }

            log.LogError($"Failed to Add Bank Account: {bankAccount.AccountNumber} as Table already contains that Account", DateTime.Now);

            return false;
        }

        public async Task<bool> DeleteBankAccount(int bankAccountId)
        {
            log.LogInformation($"Trying to Delete Bank Account with Id: {bankAccountId}", DateTime.Now);
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccountId);
            if (res != null)
            {
                log.LogInformation($"Successfully Deleted Bank Account with Id: {bankAccountId}", DateTime.Now);
                crop.BankAccounts.Remove(res);
                await crop.SaveChangesAsync();
                return true;
            }
            log.LogError($"There's no Bank Account to delete with ID:{bankAccountId}", DateTime.Now);
            return false;
        }

        public async Task<IEnumerable<BankAccount>> GetAllBankAccounts()
        {
            var res = await crop.BankAccounts.ToListAsync();
            if (res == null)
            {
                log.LogInformation("Cannot get Bank Accounts, Bank Accounts Table is Empty", DateTime.Now);
            }
            log.LogInformation("Bank Account successfully retrieved", DateTime.Now);
            return res;
        }

        public async Task<BankAccount> GetBankAccountByUser(int userId)
        {
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.UserId == userId);
            if (res == null)
            {
                log.LogInformation($"There's no Bank Account for User Id: {userId}", DateTime.Now);
            }
            log.LogInformation($"Bank Account Number for User Id: {res.AccountNumber}", DateTime.Now);
            return res;
        }

        // public async Task<bool> UpdateBankAccount(BankAccountDTO bankAccount)
        // {
        //     await  LogToDatabase("Info", $"Trying to Update Bank Account: {bankAccount.AccountNumber}", DateTime.Now);
        //     var bankAcc = mapper.Map<BankAccount>(bankAccount);
        //     var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccount.BankId);
        //     if (res == null)
        //     {
        //         res.
        //         await crop.SaveChangesAsync();
        //         await  LogToDatabase("Success", $"Successfully Added Bank Account: {bankAccount.AccountNumber}", DateTime.Now);
        //         return true;
        //     }
        //     await  LogToDatabase("Failed", $"Failed to Add Bank Account: {bankAccount.AccountNumber} as Table already contains that Account", DateTime.Now);
        //     return false;
        // }
    }
}