using System;
using System.Collections.Generic;

namespace WebShop.DatabaseEF.Entities;

public partial class ProductEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long CategoryId { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public virtual CategoryEntity Category { get; set; } = null!;
}
