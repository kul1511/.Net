using CROPDEAL.Models.DTO;
using CROPDEAL.Models;

namespace CROPDEAL.Interfaces
{
    public interface IOrders
    {
        Task LogToDatabase(string level, string message, DateTime now);
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrderById(string orderId);
        Task<bool> AddOrder(OrderDTO order);
        Task<bool> UpdateOrder(string orderId, OrderDTO order);
        Task<bool> DeleteOrder(string orderId);
        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        Task<IEnumerable<Order>> GetOrdersWithinDateRange(DateTime startDate, DateTime endDate);
    }
}