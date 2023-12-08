namespace Domain
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public DateTime AccessedAt { get; set; }
        public string SessionId { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
    }
}
