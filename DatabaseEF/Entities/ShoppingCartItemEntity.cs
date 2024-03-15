using WebShop.DatabaseEF.Entities;

namespace DatabaseEF.Entities
{
    public partial class ShoppingCartItemEntity
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public ShoppingCartEntity Cart { get; set; }
        public long ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
    }
}
