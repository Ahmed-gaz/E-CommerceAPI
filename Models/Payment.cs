namespace E_CommerceAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public int TransactionId { get; set; }
        public Order Order { get; set; }
    }
}
