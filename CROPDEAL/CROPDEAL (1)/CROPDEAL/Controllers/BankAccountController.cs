using CROPDEAL.Interfaces;
using CROPDEAL.Models;
using CROPDEAL.Repository;
using CROPDEAL.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccount bankAccount;
        public BankAccountController(IBankAccount _bankAccount) => bankAccount = _bankAccount;

        [HttpGet("GetAllBankAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBankAccounts()
        {
            try
            {
                var bankAccounts = await bankAccount.GetAllBankAccounts();
                if (bankAccounts == null)
                {
                    return NoContent();
                }
                return Ok(bankAccounts);
            }
            catch (Exception e)
            {
                await bankAccount.LogToDatabase("Failed", $"Exception Occurred. Message: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetBankAccountByUserId/{userId}")]
        public async Task<IActionResult> GetBankAccountByUserId(string userId)
        {
            try
            {
                var bankAccounts = await bankAccount.GetBankAccountByUser(userId);
                if (bankAccounts == null)
                {
                    return NoContent();
                }
                return Ok(bankAccounts);
            }
            catch (Exception e)
            {
                await bankAccount.LogToDatabase("Failed", $"Exception Occurred. Message: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("AddBankAccount")]
        public async Task<IActionResult> AddBankAccount([FromBody] BankAccountDTO bankAccountDTO)
        {
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
                await bankAccount.LogToDatabase("Failed", $"Exception Occurred. Message: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }

        //Only Admins can Delete Bank Accounts
        [HttpDelete("DeleteBankAccount/{bankId}")]
        [Authorize(Roles = "Dealer,Admin")]
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
                await bankAccount.LogToDatabase("Failed", $"Exception Occurred. Message: {e.Message}", DateTime.Now);
                return BadRequest(e.Message);
            }
        }
    }
}