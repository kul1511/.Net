using Razorpay.Api;
using Microsoft.Extensions.Configuration;

public class RazorpayService
{
    private readonly RazorpayClient _client;

    public RazorpayService(IConfiguration config)
    {
        var key = config["Razorpay:Key"];
        var secret = config["Razorpay:Secret"];
        _client = new RazorpayClient(key, secret);
    }

    public Order CreateOrder(decimal amount, string currency = "INR")
    {
        var options = new Dictionary<string, object>
        {
            { "amount", amount * 100 }, // smallest currency unit
            { "currency", currency },
            { "receipt", Guid.NewGuid().ToString() },
            { "payment_capture", 1 }
        };
        return _client.Order.Create(options);
    }
}