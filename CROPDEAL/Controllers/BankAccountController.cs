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
        private readonly ILogger<BankAccountController> log;
        private readonly IBankAccount bankAccount;
        public BankAccountController(IBankAccount _bankAccount, ILogger<BankAccountController> llog)
        {

            bankAccount = _bankAccount;
            log = llog;
        }

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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetBankAccountByUserId/{userId}")]
        public async Task<IActionResult> GetBankAccountByUserId(int userId)
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpPost("AddBankAccount")]
        public async Task<IActionResult> AddBankAccount([FromBody] BankAccountDTO bankAccountDTO)
        {
            try
            {

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim == null)
                    return Unauthorized("UserId not found in token.");

                int userId = int.Parse(userIdClaim.Value);

                bankAccountDTO.UserId = userId;

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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }

        //Only Admins can Delete Bank Accounts
        [HttpDelete("DeleteBankAccount/{bankId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> DeleteBankAccount(int bankId)
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
                log.LogError($"Exception Occurred. Message: {e.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {e.Message}. For more details see StackTrace in Log Table.");
            }
        }
    }
}