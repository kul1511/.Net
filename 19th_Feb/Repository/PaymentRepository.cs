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
using Razorpay.Api;
using CROPDEAL.Services;
using log4net;

namespace CROPDEAL.Repository
{
    public class PaymentRepository : IPayment
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PaymentRepository));
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;
        private readonly RazorpayClient _client;

        public PaymentRepository(CropDealDbContext context, IMapper mapper, IConfiguration config)
        {
            _crops = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            var key = config["Razorpay:Key"];
            var secret = config["Razorpay:Secret"];
            _client = new RazorpayClient(key, secret);
        }


        public async Task<Razorpay.Api.Order> CreateOrder(decimal amount, string currency = "INR")
        {
            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 }, // smallest currency unit
                { "currency", currency },
                { "receipt", Guid.NewGuid().ToString() },
                { "payment_capture", 1 }
            };
            _logger.Info($"Creating Razorpay order for amount: {amount} {currency}");
            return await Task.Run(() => _client.Order.Create(options));
        }


        public async Task<IEnumerable<PaymentDTO>> GetAllPayments()
        {
            _logger.Info($"Getting All Payments Details....");
            var payments = await _crops.Payments.ToListAsync();
            _logger.Info($"Retrieved {payments.Count} payments.");
            return mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }
        public async Task<PaymentDTO?> GetPaymentByOrderId(string orderId)
        {
            _logger.Info($"Getting Payment Details for Order Id: {orderId}");
            var payment = await _crops.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null)
            {
                _logger.Warn($"Payment not found for Order Id: {orderId}");
                return null;
            }
            _logger.Info($"Retrieved payment for Order Id: {orderId}");
            return mapper.Map<PaymentDTO>(payment);
        }
        public async Task<bool> MakePayment(PaymentDTO newPayment)
        {
            var orderId = await _crops.Orders.FirstOrDefaultAsync(o => o.OrderId == newPayment.OrderId);
            if (orderId == null || string.IsNullOrEmpty(orderId.OrderId))
            {
                _logger.Warn($"Order not found with Order Id: {newPayment.OrderId}");
                return false;
            }
            _logger.Info($"Order for User Id: {orderId!.UserId}");
            var payment = mapper.Map<Models.Payment>(newPayment);
            payment.UserId = orderId.UserId;
            var paymentAmount = await _crops.Orders.FirstOrDefaultAsync(o => o.OrderId == newPayment.OrderId);
            payment.Amount = paymentAmount!.Price;
            await _crops.Payments.AddAsync(payment);
            await _crops.SaveChangesAsync();
            _logger.Info($"Successfully Added Payment with Id: {newPayment.PaymentId}");
            var orderStatus = await _crops.Orders.FirstOrDefaultAsync(c => c.OrderId == newPayment.OrderId);
            orderStatus!.Status = "Delivered";
            await _crops.SaveChangesAsync();
            _logger.Info($"Successfully Changed Order Status");
            //Adding Invoice after Successfuly Payment
            _logger.Info($"Creating Invoice....");
            var Invoice = new Models.Invoice
            {
                InvoiceId = String.Concat('I', newPayment.PaymentId),
                OrderId = newPayment.OrderId,
                UserId = payment.UserId,
                PaymentTime = newPayment.PaymentDate,
                TotalAmount = payment.Amount
            };
            if (await _crops.Invoices.AnyAsync(i => i.InvoiceId == Invoice.InvoiceId))
            {
                _logger.Warn($"Invoice Already Present with Invoice Id: {Invoice.InvoiceId}");
                return false;
            }
            var dealer = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == payment.UserId);
            if (dealer == null)
            {
                _logger.Warn($"Dealer not found for User Id: {payment.UserId}");
                return false;
            }
            var crop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == orderId.CropId);
            if (crop == null)
            {
                _logger.Warn($"Crop not found for Crop Id: {orderId.CropId}");
                return false;
            }
            var farmerEmail = await _crops.Users.FirstOrDefaultAsync(f => f.UserId == crop.UserId);
            if (farmerEmail == null)
            {
                _logger.Warn($"Farmer not found for User Id: {crop.UserId}");
                return false;
            }
            _logger.Info($"Invoice for Farmer: {farmerEmail.Email}, Crop: {crop.CropId}");
            await _crops.Invoices.AddAsync(Invoice);
            await _crops.SaveChangesAsync();
            farmerEmail.Email = farmerEmail.Email!.Trim();
            var sendInvoiceEmail = new EmailNotification();
            await sendInvoiceEmail.SendInvoice(dealer.FullName!, farmerEmail.Email, orderId);
            _logger.Info($"Invoice Created with Invoice Id: {Invoice.InvoiceId}");
            return true;
        }
    }
}