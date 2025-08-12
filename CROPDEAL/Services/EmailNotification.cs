using MailKit.Net.Smtp;
using MimeKit;
using CROPDEAL.Models;

namespace CROPDEAL.Services
{
    public class EmailNotification
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromName = "CropDeal Notifications";
        private readonly string _fromPassword = "glbu lfbz fnnx falf";

        public async Task SendCropNotificationAsync(string farmerName, string fromEmail, List<string> toEmails, Crop crop)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, fromEmail));

            // Add all recipients
            foreach (var email in toEmails)
            {
                message.To.Add(MailboxAddress.Parse(email));
            }

            message.Subject = $"New Crop Listed by {farmerName}";

            message.Body = new TextPart("plain")
            {
                Text = $"Hello,\n\nA new crop has just been listed for sale:\n" +
                       $"- Crop Name: {crop.CropType}\n" +
                       $"- Price: ₹{crop.PricePerUnit}/kg\n\n" +
                       $"Log in to CropDeal now to contact the farmer or make an offer!\n\n" +
                       $"Happy Dealing"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("rajputsinghkuldeep15@gmail.com", _fromPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendInvoice(string dealerName, string toEmail, Order order)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, dealerName));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Order Invoice for Order Id: {order.OrderId}";

            message.Body = new TextPart("plain")
            {
                Text = $"Invoice for Order Id: {order.OrderId}\n" +
                       $"Crop Id: {order.Crop.CropId}\n" +
                       $"- Crop Name: {order.Crop.CropType}\n" +
                       $"- Quantity: {(int)order.Quantity}\n" +
                       $"- Price: ₹{order.Price}\n\n" +
                       $"- Date and Time: {order.OrderDate}\n\n" +
                       $"- Location: {order.Crop.Location}\n\n" +
                       $"Thank You"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync("rajputsinghkuldeep15@gmail.com", _fromPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}