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
        private readonly CropDealDbContext _context;
        private readonly IMapper _mapper;

        public InvoiceRepository(CropDealDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            return invoices;
        }
        public async Task<Invoice> GetInvoiceById(string invoiceId)
        {
            var invoice = await _context.Invoices.Include(o => o.Order).Include(u => u.User).FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
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
            if (invoice == null) return false;

            _context.Invoices.Remove(invoice);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}