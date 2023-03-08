using System;
using System.Collections.Generic;

namespace WebShop.DatabaseEF.Entities;

public partial class CategoryEntity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductEntity> Products { get; } = new List<ProductEntity>();
}
