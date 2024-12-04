using Microsoft.Extensions.Configuration;
using Stripe;
using System.Threading.Tasks;

public class PaymentService
{
    public PaymentService(IConfiguration configuration)
    {
        var secretKey = configuration["Stripe:SecretKey"];
        StripeConfiguration.ApiKey = secretKey;
    }

    public async Task<PaymentResult> ProcessPaymentAsync(string paymentMethodId, decimal amount, string returnUrl, string receiptEmail)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "thb",
            PaymentMethod = paymentMethodId,
            Confirm = true, 
            ReturnUrl = returnUrl, 
            ReceiptEmail = receiptEmail 
        };

        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return new PaymentResult
            {
                IsSuccess = true,
                PaymentIntentId = paymentIntent.Id
            };
        }
        catch (StripeException ex)
        {
            return new PaymentResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string PaymentIntentId { get; set; }
    public string ErrorMessage { get; set; }
}
