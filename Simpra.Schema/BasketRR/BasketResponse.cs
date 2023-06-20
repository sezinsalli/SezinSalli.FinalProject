﻿namespace Simpra.Schema.BasketRR
{
    public class BasketResponse
    {
        public List<BasketItemRequest> BasketItems { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(x => x.UnitPrice * x.Quantity);
        }
    }
}
