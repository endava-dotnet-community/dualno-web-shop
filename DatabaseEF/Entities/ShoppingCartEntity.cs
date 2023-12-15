namespace DatabaseEF.Entities
{
    public partial class ShoppingCartEntity
    {
        public long Id { get; set; }
        public DateTime AccessedAt { get; set; }
        public string SessionId { get; set; }
        public virtual ICollection<ShoppingCartItemEntity> Items { get; set; } = new List<ShoppingCartItemEntity>();
    }
}
