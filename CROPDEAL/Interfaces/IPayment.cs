using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CROPDEAL.Models.DTO;

namespace CROPDEAL.Interfaces
{
    public interface IPayment
    {
        public Task LogToDatabase(string level, string message, DateTime now);
        public Task<string> MakePayment(string orderId);
        public Task<string> MarkPaymentSuccess(string paymentId);
        public Task<string> CheckPaymentStatus(string paymentId);
    }
}