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
        private readonly CropDealDbContext crop;
        private readonly IMapper mapper;
        public BankAccountRepository(CropDealDbContext user, IMapper _mapper)
        {
            crop = user;
            mapper = _mapper;
        }

        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await crop.Logs.AddAsync(log);
            await crop.SaveChangesAsync();
        }
        public async Task<bool> AddBankAccount(BankAccountDTO bankAccount)
        {
            await LogToDatabase("Info", $"Trying to Add Bank Account: {bankAccount.AccountNumber}", DateTime.Now);
            var bankAcc = mapper.Map<BankAccount>(bankAccount);
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccount.BankId);
            if (res == null)
            {
                await crop.BankAccounts.AddAsync(bankAcc);
                await crop.SaveChangesAsync();
                await LogToDatabase("Success", $"Successfully Added Bank Account: {bankAccount.AccountNumber}", DateTime.Now);
                return true;
            }
            await LogToDatabase("Failed", $"Failed to Add Bank Account: {bankAccount.AccountNumber} as Table already contains that Account", DateTime.Now);
            return false;
        }

        public async Task<bool> DeleteBankAccount(string bankAccountId)
        {
            await LogToDatabase("Info", $"Trying to Delete Bank Account with Id: {bankAccountId}", DateTime.Now);
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccountId);
            if (res != null)
            {
                await LogToDatabase("Success", $"Successfully Deleted Bank Account with Id: {bankAccountId}", DateTime.Now);
                crop.BankAccounts.Remove(res);
                await crop.SaveChangesAsync();
                return true;
            }
            await LogToDatabase("Failed", $"There's no Bank Account to delete with ID:{bankAccountId}", DateTime.Now);
            return false;
        }

        public async Task<IEnumerable<BankAccount>> GetAllBankAccounts()
        {
            var res = await crop.BankAccounts.ToListAsync();
            if (res == null)
            {
                await LogToDatabase("Info", "Bank Accounts Table is Empty", DateTime.Now);
            }
            await LogToDatabase("Success", "Bank Account successfully retrieved", DateTime.Now);
            return res;
        }

        public async Task<BankAccount> GetBankAccountByUser(string userId)
        {
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.UserId == userId);
            if (res == null)
            {
                await LogToDatabase("Info", $"There's no Bank Account for User Id: {userId}", DateTime.Now);
            }
            await LogToDatabase("Success", $"Bank Account Number for User Id: {res.AccountNumber}", DateTime.Now);
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