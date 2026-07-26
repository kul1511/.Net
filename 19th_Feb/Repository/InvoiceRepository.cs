using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Interfaces;
using AutoMapper;
using CROPDEAL.Data;
using CROPDEAL.Models;
using CROPDEAL.Models.DTO;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace CROPDEAL.Repository
{
    public class InvoiceRepository : IInvoice
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(InvoiceRepository));
        private readonly CropDealDbContext _context;
        private readonly IMapper _mapper;

        public InvoiceRepository(CropDealDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            var invoices = await _context.Invoices.Include(o => o.Order).Include(u => u.User).ToListAsync();
            _logger.Info($"Retrieved {invoices.Count} invoices.");
            return invoices;
        }
        public async Task<Invoice> GetInvoiceById(string invoiceId)
        {
            var invoice = await _context.Invoices.Include(o => o.Order).Include(u => u.User).FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            if (invoice == null)
            {
                _logger.Warn($"Invoice not found for Id: {invoiceId}");
                return new Invoice();
            }
            _logger.Info($"Retrieved invoice for Id: {invoiceId}");
            return invoice!;
        }
        public async Task<bool> AddInvoice(InvoiceDTO invoiceDTO)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDTO);
            await _context.Invoices.AddAsync(invoice);
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
                _logger.Info($"Invoice added successfully for Id: {invoiceDTO.InvoiceId}");
            else
                _logger.Warn($"Failed to add invoice for Id: {invoiceDTO.InvoiceId}");
            return result;
        }
        public async Task<bool> DeleteInvoice(string invoiceId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            if (invoice == null)
            {
                _logger.Warn($"Invoice not found for delete Id: {invoiceId}");
                return false;
            }
            _context.Invoices.Remove(invoice);
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
                _logger.Info($"Invoice deleted successfully for Id: {invoiceId}");
            else
                _logger.Warn($"Failed to delete invoice for Id: {invoiceId}");
            return result;
        }

        public async Task<IEnumerable<object>> GetDeliveredCropsForDealer(string dealerId)
        {
            var deliveredCrops = await _context.Invoices
                .Include(i => i.Order)
                    .ThenInclude(o => o.Crop)
                .Include(i => i.User)
                .Where(i => i.UserId == dealerId)
                .Select(i => new
                {
                    InvoiceId = i.InvoiceId,
                    OrderId = i.OrderId,
                    CropType = i.Order.Crop.CropType,
                    Quantity = i.Order.Quantity,
                    Price = i.Order.Price,
                    TotalAmount = i.TotalAmount,
                    PaymentTime = i.PaymentTime,
                    OrderDate = i.Order.OrderDate,
                    Location = i.Order.Crop.Location
                })
                .ToListAsync();

            _logger.Info($"Retrieved {deliveredCrops.Count} delivered crops for dealer: {dealerId}");
            return deliveredCrops;
        }
    }
}