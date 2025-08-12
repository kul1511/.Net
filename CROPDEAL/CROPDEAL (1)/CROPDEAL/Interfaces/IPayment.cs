using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IPayment
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<PaymentDTO>> GetAllPayments();
        Task<PaymentDTO?> GetPaymentByOrderId(string orderId);
        Task<bool> MakePayment(PaymentDTO paymentDTO);
    }
}