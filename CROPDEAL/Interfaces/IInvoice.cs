using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models.DTO;
using CROPDEAL.Models;

namespace CROPDEAL.Interfaces
{
    public interface IInvoice
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<Invoice>> GetAllInvoices();
        Task<Invoice> GetInvoiceById(string invoiceId);
        Task<bool> AddInvoice(InvoiceDTO invoiceDTO);
        Task<bool> DeleteInvoice(string invoiceId);
    }
}