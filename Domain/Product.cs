﻿namespace Domain
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}