using System.ComponentModel.DataAnnotations;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Models.DTO;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace CROPDEAL.Repository
{
    public class BankAccountRepository : IBankAccount
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BankAccountRepository));
        private readonly CropDealDbContext crop;
        private readonly IMapper mapper;
        public BankAccountRepository(CropDealDbContext user, IMapper _mapper)
        {
            crop = user;
            mapper = _mapper;
        }

        public async Task<bool> AddBankAccount(BankAccountDTO bankAccount)
        {
            var checkBankAccountNumber=new BankAccount();
            do
            {
                long bankAccountNumber = new Random().Next(10000000, 100000000);

                checkBankAccountNumber = await crop.BankAccounts.FindAsync(bankAccountNumber);

                if (checkBankAccountNumber != null)
                {
                    _logger.Info($"Account Number already exists: {bankAccountNumber}. Generating a new Account Number");
                }
            } while (checkBankAccountNumber == null);

            _logger.Info($"Trying to Add Bank Account: {bankAccount.AccountNumber}");

            var bankAcc = mapper.Map<BankAccount>(bankAccount);
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccount.BankId);

            if (res == null)
            {
                await crop.BankAccounts.AddAsync(bankAcc);
                await crop.SaveChangesAsync();
                _logger.Info($"Successfully Added Bank Account: {bankAccount.AccountNumber}");
                return true;
            }
            _logger.Warn($"Failed to Add Bank Account: {bankAccount.AccountNumber} as Table already contains that Account");
            return false;
        }

        public async Task<bool> DeleteBankAccount(string bankAccountId)
        {
            _logger.Info($"Trying to Delete Bank Account with Id: {bankAccountId}");
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.BankId == bankAccountId);
            if (res != null)
            {
                _logger.Info($"Successfully Deleted Bank Account with Id: {bankAccountId}");
                crop.BankAccounts.Remove(res);
                await crop.SaveChangesAsync();
                return true;
            }
            _logger.Warn($"There's no Bank Account to delete with ID:{bankAccountId}");
            return false;
        }

        public async Task<IEnumerable<BankAccount>> GetAllBankAccounts()
        {
            var res = await crop.BankAccounts.ToListAsync();
            if (res == null)
            {
                _logger.Info("Bank Accounts Table is Empty");
            }
            _logger.Info("Bank Account successfully retrieved");
            return res!;
        }

        public async Task<BankAccountDTO> GetBankAccountByUser(string userEmail)
        {
            var res = await crop.BankAccounts.FirstOrDefaultAsync(b => b.User!.Email == userEmail);
            if (res == null)
            {
                _logger.Info($"There's no Bank Account for User with Email: {userEmail}");
                return null!;
            }
            _logger.Info($"Bank Account Number for User Id: {res.AccountNumber}");
            return new BankAccountDTO
            {
                BankId = res.BankId,
                BankName = res.BankName,
                IFSCCode = res.IFSCCode,
                UserId = res.UserId
            };
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