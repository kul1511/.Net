using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using CROPDEAL.Models;
using AutoMapper;
using CROPDEAL.Data;

namespace CROPDEAL.Repository
{
    public class PaymentRepository : IPayment
    {
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;

        public PaymentRepository(CropDealDbContext user, IMapper _mapper)
        {
            _crops = user;
            mapper = _mapper;
        }
        public async Task LogToDatabase(string level, string message, DateTime now)
        {
            var log = new Log
            {
                LogLevel = level,
                Message = message,
                Timestamp = now
            };
            await _crops.Logs.AddAsync(log);
            await _crops.SaveChangesAsync();
        }
        public async Task<IEnumerable<PaymentDTO>> GetAllPayments()
        {
            await LogToDatabase("Info", $"Getting All Crops Details....", DateTime.Now);
            var payments = await _crops.Payments.ToListAsync();
            return mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }
        public async Task<PaymentDTO?> GetPaymentByOrderId(string orderId)
        {
            await LogToDatabase("Info", $"Getting All Crops Details....", DateTime.Now);
            var payment = await _crops.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null) return null;
            return mapper.Map<PaymentDTO>(payment);
        }
        public async Task<bool> MakePayment(PaymentDTO newPayment)
        {
            var payment = mapper.Map<Payment>(newPayment);

            var paymentAmount = await _crops.Orders.FirstOrDefaultAsync(o => o.OrderId == newPayment.OrderId);

            payment.Amount = paymentAmount.Price;

            await _crops.Payments.AddAsync(payment);
            await _crops.SaveChangesAsync();
            await LogToDatabase("Success", $"Successfully Added Payment with Id: {newPayment.PaymentId}", DateTime.Now);

            var orderStatus = await _crops.Orders.FirstOrDefaultAsync(c => c.OrderId == newPayment.OrderId);

            orderStatus.Status = "Delivered";
            await _crops.SaveChangesAsync();
            await LogToDatabase("Success", $"Successfully Changes Order Status", DateTime.Now);

            //Adding Invoice after Successfuly Payment
            await LogToDatabase("Info", "Creating Invoice....", DateTime.Now);
            var Invoice = new Invoice
            {
                InvoiceId = String.Concat('I', newPayment.PaymentId),
                OrderId = newPayment.OrderId,
                UserId = newPayment.UserId,
                PaymentTime = newPayment.PaymentDate,
                TotalAmount = payment.Amount
            };
            if (await _crops.Invoices.AnyAsync(i => i.InvoiceId == Invoice.InvoiceId))
            {
                await LogToDatabase("Fail", $"Invoice Already Present with Invoice Id: {Invoice.InvoiceId}", DateTime.Now);
                return false;
            }
            await _crops.Invoices.AddAsync(Invoice);
            await LogToDatabase("Info", $"Invoice Created with Invoice Id: {Invoice.InvoiceId}", DateTime.Now);

            return true;
        }
    }
}