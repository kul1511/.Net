using Stripe;
using CROPDEAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models;
using CROPDEAL.Data;
using CROPDEAL.Services;
using Stripe.Climate;

public class PaymentRepository : IPayment
{
    private readonly ILogger<PaymentRepository> log;
    private readonly CropDealDbContext _crops;
    private readonly string _stripeApiKey = "sk_test_51RD37xQghkM3FluWKM8rwxqqkSBXLEyuL8ZtXgGOGjQ6KrtpJL9QbMWuDlbmI6mPdMUmuXfJi1EkApnSI97IscLQ00W0UO2rc3";

    public PaymentRepository(CropDealDbContext crop, ILogger<PaymentRepository> _log)
    {
        _crops = crop;
        StripeConfiguration.ApiKey = _stripeApiKey;
        log = _log;
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
    public async Task<string> MakePayment(string orderId)
    {
        var order = await _crops.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null)
        {
            log.LogError($"Order not found for Id: {orderId}", DateTime.Now);
            return "Order not found";
        }

        var paymentIntentOptions = new PaymentIntentCreateOptions
        {
            Amount = (long)(order.Price * 100),
            Currency = "inr",
            PaymentMethod = "pm_card_visa",
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
                AllowRedirects = "never"
            },
            Confirm = true,
            Description = $"Payment for OrderId: {order.OrderId}"
        };

        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

        var newPayment = new Payment
        {
            PaymentId = paymentIntent.Id,
            OrderId = orderId,
            UserId = order.UserId,
            Amount = order.Price,
            PaymentStatus = "Pending",
            TransactionDate = DateTime.UtcNow
        };

        await _crops.Payments.AddAsync(newPayment);
        await _crops.SaveChangesAsync();

        order.Status = "Paid";
        await _crops.SaveChangesAsync();
        log.LogInformation($"Status changed for Order: {order.OrderId}", DateTime.Now);

        log.LogInformation($"Payment Created with Payment Id: {paymentIntent.Id}", DateTime.Now);

        // Adding Invoice after Successful Payment
        log.LogInformation("Creating Invoice....", DateTime.Now);
        var Invoice = new CROPDEAL.Models.Invoice
        {
            InvoiceId = String.Concat('I', newPayment.PaymentId),
            OrderId = newPayment.OrderId,
            UserId = newPayment.UserId,
            PaymentTime = newPayment.TransactionDate,
            TotalAmount = newPayment.Amount
        };
        if (await _crops.Invoices.AnyAsync(i => i.InvoiceId == Invoice.InvoiceId))
        {
            log.LogError($"Invoice Already Present with Invoice Id: {Invoice.InvoiceId}", DateTime.Now);
            return $"Invoice Already Present with Invoice Id: {Invoice.InvoiceId}";
        }

        var dealer = await _crops.Users.FirstOrDefaultAsync(u => u.UserId == newPayment.UserId);
        var crop = await _crops.Crops.FirstOrDefaultAsync(u => u.CropId == order.CropId);
        var farmerEmail = await _crops.Users.FirstOrDefaultAsync(f => f.UserId == crop.UserId);

        await _crops.Invoices.AddAsync(Invoice);
        await _crops.SaveChangesAsync();

        var sendInvoiceEmail = new EmailNotification();
        await sendInvoiceEmail.SendInvoice(dealer.FullName, farmerEmail.Email, order);
        log.LogInformation($"Invoice Created with Invoice Id: {Invoice.InvoiceId}", DateTime.Now);
        log.LogInformation($"Stripe PaymentIntent created with ID: {paymentIntent.Id}", DateTime.Now);

        return $"Stripe PaymentIntent Created: {paymentIntent.Id}";
    }



    public async Task<string> CheckPaymentStatus(string paymentId)
    {
        var service = new PaymentIntentService();
        var paymentIntent = await service.GetAsync(paymentId);

        var payment = await _crops.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        if (payment == null)
            return "Payment not found";

        payment.PaymentStatus = paymentIntent.Status == "succeeded" ? "Success" : "Pending";
        await _crops.SaveChangesAsync();

        return $"Payment Status: {payment.PaymentStatus}";
    }

    public async Task<string> MarkPaymentSuccess(string paymentId)
    {
        string status = "Success";
        var service = new PaymentIntentService();

        try
        {
            var paymentIntent = await service.GetAsync(paymentId);

            if (paymentIntent.Status == "requires_capture")
            {
                var capturedIntent = await service.CaptureAsync(paymentId);

                var payment = await _crops.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
                if (payment != null)
                {
                    payment.PaymentStatus = "Success";
                    await _crops.SaveChangesAsync();
                }

                log.LogInformation($"PaymentIntent {paymentId} captured successfully.", DateTime.Now);
                return $"PaymentIntent {paymentId} marked as succeeded.";
            }
            else
            {
                log.LogInformation($"PaymentIntent {paymentId} is not in a capturable state: {paymentIntent.Status}", DateTime.Now);
                return $"PaymentIntent {paymentId} is not in a capturable state: {paymentIntent.Status}";
            }
        }
        catch (StripeException ex)
        {
            log.LogError($"Stripe error while capturing PaymentIntent {paymentId}: {ex.Message}", DateTime.Now);
            return $"Error capturing PaymentIntent {paymentId}: {ex.Message}";
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error while capturing PaymentIntent {paymentId}: {ex.Message}", DateTime.Now);
            return $"Unexpected error capturing PaymentIntent {paymentId}: {ex.Message}";
        }
    }


}
