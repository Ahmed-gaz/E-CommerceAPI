namespace E_CommerceAPI.Models
{
    public class Orderr
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string State {  get; set; }
        public int UserId { get; set; }
        public int? PaymentId { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; } 
        public User User { get; set; }
        public Payment Payment { get; set; }
    }
}
