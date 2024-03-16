using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.Tests
{
    public class Helper
    {
        public static Cart CreateCart(string country, string city, ShippingMethod shippingMethod, CustomerType customerType, uint quantity)
        {
            Address address = new Address
            {
                Country = country,
                City = city,
                Street = "1234 left"
            };
            Cart cart = new Cart
            {
                ShippingAddress = address,
                ShippingMethod = shippingMethod,
                CustomerType = customerType,
                Items = new System.Collections.Generic.List<Item>
                {
                    new Item
                    {
                        ProductId = "1",
                        ProductName = "Product 1",
                        Price = 10,
                        Quantity = quantity
                    }
                }
            };
            return cart;
        }
    }
}