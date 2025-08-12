using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Models.DTO;
using CROPDEAL.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoice invoiceService;
        private readonly ILogger<InvoiceController> log;

        public InvoiceController(IInvoice _invoiceService, ILogger<InvoiceController> _logger)
        {
            invoiceService = _invoiceService;
            log = _logger;
        }

        [HttpGet("GetAllInvoices")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await invoiceService.GetAllInvoices();
                if (invoices == null)
                {
                    return NoContent();
                }
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        [HttpGet("GetInvoiceById/{invoiceId}")]
        [Authorize(Roles = "Farmer,Admin")]
        public async Task<IActionResult> GetInvoiceById(string invoiceId)
        {
            try
            {
                var invoice = await invoiceService.GetInvoiceById(invoiceId);
                if (invoice == null)
                    return NotFound("Invoice not found");

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }

        // [HttpPost("AddInvoice")]
        // public async Task<IActionResult> AddInvoice([FromBody] InvoiceDTO invoiceDTO)
        // {
        //     try
        //     {
        //         if (await invoiceService.AddInvoice(invoiceDTO))
        //             return Ok("Invoice added successfully");

        //         return BadRequest("Failed to add invoice");
        //     }
        //     catch (Exception ex)
        //     {
        //         await invoiceService.LogToDatabase("Error", $"Exception Occurred: {ex.StackTrace}", DateTime.Now);
        //         return BadRequest(ex.StackTrace);
        //     }
        // }

        [HttpDelete("DeleteInvoiceById/{invoiceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInvoice(string invoiceId)
        {
            try
            {
                if (await invoiceService.DeleteInvoice(invoiceId))
                    return Ok("Invoice deleted successfully");

                return BadRequest("Failed to delete invoice");
            }
            catch (Exception ex)
            {
                log.LogError($"Exception Occurred. Message: {ex.StackTrace}", DateTime.Now);
                return BadRequest($"Error Occurred: {ex.Message}. For more details see StackTrace in Log Table.");
            }
        }
    }
}