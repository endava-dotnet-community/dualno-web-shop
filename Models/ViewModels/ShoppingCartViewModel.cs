namespace Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public long Id { get; set; }
        public DateTime AccessedAt { get; set; }
        public string SessionId { get; set; }
        public List<ShoppingCartItemViewModel> Items { get; set; }
    }
}
