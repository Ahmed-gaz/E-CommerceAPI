namespace E_CommerceAPI.Models
{

    public enum PaymentMethod
    {
        Card,
        Stripe,
        PayPal,
        CashOnDelivery
    }

    public enum PaymentStatus
    {
        Pending,
        Success,
        Failed,
        Refunded
    }

    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentMethod Method { get; set; }
        public int Amount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }

        public Orderr Order { get; set; }
    }
}
