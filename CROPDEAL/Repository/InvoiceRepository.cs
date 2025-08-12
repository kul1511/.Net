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

namespace CROPDEAL.Repository
{
    public class InvoiceRepository : IInvoice
    {
        private readonly ILogger<InvoiceRepository> log;
        private readonly CropDealDbContext _context;
        private readonly IMapper _mapper;

        public InvoiceRepository(CropDealDbContext context, IMapper mapper, ILogger<InvoiceRepository> llog)
        {
            _context = context;
            _mapper = mapper;
            log = llog;
        }
        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Invoice>> GetAllInvoices()
        {
            var invoices = await _context.Invoices.Include(o => o.Order).Include(u => u.User).ToListAsync();
            if (invoices == null)
            {
                log.LogError("Cannot get Invoices, Invoices Table is Empty", DateTime.Now);
            }
            return invoices;
        }
        public async Task<Invoice> GetInvoiceById(string invoiceId)
        {
            var invoice = await _context.Invoices.Include(o => o.Order).Include(u => u.User).FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            if (invoice == null)
            {
                log.LogError($"Invoice with Id: {invoiceId} is not Present", DateTime.Now);
            }
            return invoice;
        }
        public async Task<bool> AddInvoice(InvoiceDTO invoiceDTO)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDTO);
            await _context.Invoices.AddAsync(invoice);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> DeleteInvoice(string invoiceId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
            if (invoice == null)
            {
                log.LogError($"Unable to Delete Invoice: {invoiceId}. Invoice is not Present", DateTime.Now);
                return false;

            }

            _context.Invoices.Remove(invoice);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}