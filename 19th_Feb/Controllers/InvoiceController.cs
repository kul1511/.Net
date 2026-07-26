using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CROPDEAL.Interfaces;
using CROPDEAL.Models.DTO;
using CROPDEAL.Repository;
using Microsoft.AspNetCore.Authorization;
using log4net;

namespace CROPDEAL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoice invoiceService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(InvoiceController));
        public InvoiceController(IInvoice _invoiceService)
        {
            invoiceService = _invoiceService;
        }
        [HttpGet("GetAllInvoices")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoices = await invoiceService.GetAllInvoices();
                _logger.Info($"Fetched all invoices.");
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
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
                {
                    _logger.Warn($"Invoice not found for InvoiceId: {invoiceId}");
                    return NotFound();
                }
                _logger.Info($"Fetched invoice for InvoiceId: {invoiceId}");
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
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
        //         _logger.Error($"Exception Occurred: {ex.Message}");
        //         return BadRequest(ex.Message);
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
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDeliveredCrops/{dealerId}")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetDeliveredCropsForDealer(string dealerId)
        {
            try
            {
                var deliveredCrops = await invoiceService.GetDeliveredCropsForDealer(dealerId);
                _logger.Info($"Fetched delivered crops for dealer: {dealerId}");
                return Ok(deliveredCrops);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}