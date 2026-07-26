using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IPayment
    {
        Task<Razorpay.Api.Order> CreateOrder(decimal amount, string currency = "INR");
        Task<IEnumerable<PaymentDTO>> GetAllPayments();
        Task<PaymentDTO?> GetPaymentByOrderId(string orderId);
        Task<bool> MakePayment(PaymentDTO paymentDTO);

    }
}