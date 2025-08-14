namespace E_CommerceAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string State {  get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }

        public User User { get; set; }
        public Payment Payment { get; set; }
    }
}
