using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models.DTO;
using CROPDEAL.Models;
using AutoMapper;
using CROPDEAL.Data;
using CROPDEAL.Interfaces;

namespace CROPDEAL.Repository
{
    public class OrderRepository : IOrders
    {
        ILogger<OrderRepository> log;
        private readonly IMapper mapper;
        private readonly CropDealDbContext _crops;
        public OrderRepository(CropDealDbContext user, IMapper _mapper, ILogger<OrderRepository> llog)
        {
            _crops = user;
            mapper = _mapper;
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
            await _crops.Logs.AddAsync(log);
            await _crops.SaveChangesAsync();
        }

        public static string GenerateOrderNumber()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _crops.Orders.ToListAsync();
            return mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO?> GetOrderById(string orderId)
        {
            var order = await _crops.Orders.FindAsync(orderId);
            if (order == null) return null;
            return mapper.Map<OrderDTO>(order);
        }

        public async Task<bool> AddOrder(OrderDTO newOrder)
        {
            if (!DateTime.TryParse(newOrder.OrderDate.ToString(), out DateTime parsedDate))
            {
                log.LogError("Invalid date format for OrderDate.", DateTime.Now);
                return false;
            }

            newOrder.OrderDate = parsedDate;
            newOrder.OrderNumber = GenerateOrderNumber();

            var subscription = await _crops.Subscriptions
                .FirstOrDefaultAsync(s => s.SubscriptionId == newOrder.SubscriptionId && s.UserId == newOrder.UserId);

            if (subscription == null)
            {
                log.LogError("Subscription not found. Order cannot be placed.", DateTime.Now);
                return false;
            }

            var order = mapper.Map<Order>(newOrder);
            var checkOrder = await _crops.Orders.FirstOrDefaultAsync(o => o.OrderId == newOrder.OrderId);

            var crop = await _crops.Crops.FirstOrDefaultAsync(c => c.CropId == order.CropId);

            var orderPrice = newOrder.Quantity * crop.PricePerUnit;

            order.Price = orderPrice;

            order.Status = "Pending";

            var orderedCrop = await _crops.Crops.FirstOrDefaultAsync(c => c.CropId == newOrder.CropId);
            if (newOrder.Quantity > orderedCrop.Quantity)
            {
                log.LogError("Order Crop Quantity is greater than the available quantity", DateTime.Now);
                return false;
            }
            var updatedQuantity = orderedCrop.Quantity - newOrder.Quantity;

            if (checkOrder == null)
            {
                await _crops.Orders.AddAsync(order);
                await _crops.SaveChangesAsync();
                log.LogInformation($"Successfully Added Order with Id: {newOrder.OrderId}", DateTime.Now);
                orderedCrop.Quantity = (int)updatedQuantity;
                await _crops.SaveChangesAsync();
                log.LogInformation($"Successfully Updated Crops Quantity", DateTime.Now);
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateOrder(string orderId, OrderDTO updatedOrder)
        {
            var order = await _crops.Orders.FindAsync(orderId);
            if (order == null) return false;

            mapper.Map(updatedOrder, order);

            _crops.Orders.Update(order);

            await _crops.SaveChangesAsync();

            log.LogInformation($"Successfully Updated Order with Id: {updatedOrder.OrderId}", DateTime.Now);

            return true;
        }
        public async Task<bool> DeleteOrder(string orderId)
        {
            var order = await _crops.Orders.FindAsync(orderId);

            if (order == null) return false;

            _crops.Orders.Remove(order);
            await _crops.SaveChangesAsync();

            log.LogInformation($"Successfully Deleted Order with Id: {orderId}", DateTime.Now);

            return true;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(int userId)
        {
            var userRole = await _crops.Users.FirstOrDefaultAsync(o => o.UserId == userId);

            if (userRole.Role != UserRole.Dealer)
            {
                log.LogError("Trying to get Orders for Farmer or Admin which is not allowed", DateTime.Now);
                return null;
            }

            var orderByUserId = await _crops.Orders.Where(c => c.UserId == userId).ToListAsync();

            if (orderByUserId != null)
            {
                log.LogInformation($"Recieved {orderByUserId.Count} orders for Dealer with Id: {userId}", DateTime.Now);
                return orderByUserId;
            }
            log.LogInformation($"Table does not contains Orders for Dealer with Id: {userId}", DateTime.Now);
            return null;
        }

        public async Task<IEnumerable<Order>> GetOrdersWithinDateRange(DateTime startDate, DateTime endDate)
        {
            var orders = await _crops.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToListAsync();
            if (orders == null)
            {
                log.LogInformation($"There are no orders within {startDate} and {endDate}", DateTime.Now);
                return null;
            }
            return orders;
        }
    }
}