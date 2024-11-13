using ePizzaHub.Core.Entities;
using Razorpay.Api;

namespace ePizzaHub.Services.Interfaces
{
    public interface IPaymentService : IService<PaymentDetail>
    {
        string CreateOrder(decimal amount, string currency, string receipt);
        Payment GetPaymentDetails(string paymentId);
        bool VerifySignature(string signature, string orderId, string paymentId);
        int SavePaymentDetails(PaymentDetail model);
    }
}
