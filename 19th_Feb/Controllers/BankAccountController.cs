using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Repository;
using CROPDEAL.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CROPDEAL.Data;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccount bankAccount;
        private readonly CropDealDbContext context;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BankAccountController));
        public BankAccountController(IBankAccount _bankAccount, CropDealDbContext _context)
        {
            bankAccount = _bankAccount;
            context = _context;
        }

        [HttpGet("GetAllBankAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBankAccounts()
        {
            try
            {
                var bankAccounts = await bankAccount.GetAllBankAccounts();
                if (bankAccounts == null || bankAccounts.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(bankAccounts);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred. Message: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetBankAccountByUserId/{userEmail}")]
        public async Task<IActionResult> GetBankAccountByUserId(string userEmail)
        {
            try
            {
                var bankAccounts = await bankAccount.GetBankAccountByUser(userEmail);
                if (bankAccounts == null)
                {
                    return NotFound($"No Bank Account found for User: {userEmail}");
                }
                return Ok(bankAccounts);
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred. Message: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddBankAccount")]
        public async Task<IActionResult> AddBankAccount(string BankId, string BankName, string IFSCCode, string userEmail)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Email==userEmail);
            if (user == null)
            {
                return NotFound("Unable to added Bank Account. User Not Found");
            }

            BankAccountDTO bankAccountDTO = new BankAccountDTO
            {
                BankId = BankId,
                BankName = BankName,
                IFSCCode = IFSCCode.ToUpper(),
                UserId = user.UserId
            };
            try
            {
                if (bankAccountDTO == null)
                {
                    return BadRequest("Request is Null");
                }
                var bankAccounts = await bankAccount.AddBankAccount(bankAccountDTO);
                if (bankAccounts)
                {
                    return Ok("Successfully Added Bank Account");
                }
                else
                {
                    return BadRequest("Already Bank Account is Present");
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred. Message: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        //Only Admins can Delete Bank Accounts
        [HttpDelete("DeleteBankAccount/{bankId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBankAccount(string bankId)
        {
            try
            {
                if (bankId == null)
                {
                    return BadRequest("Bank Id in Request is null");
                }
                var bankAccounts = await bankAccount.DeleteBankAccount(bankId);
                if (bankAccounts)
                {
                    return Ok($"Successfully Deleted Bank Account with ID: {bankId}");
                }
                else
                {
                    return BadRequest($"There's no Bank Account with ID: {bankId}");
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Exception Occurred. Message: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}