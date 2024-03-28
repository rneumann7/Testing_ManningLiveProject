using System.Collections.Generic;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
    {

        private Cart CreateCart(
            CustomerType customerType = CustomerType.Standard,
            ShippingMethod shippingMethod = ShippingMethod.Standard,
            List<Item> items = null,
            Address shippingAddress = null)
        {
            items = items ?? new List<Item> { new Item { Quantity = 1 } };
            shippingAddress = shippingAddress ?? new Address { City = "city", Country = "country", Street = "street" };
            return new Cart
            {
                CustomerType = customerType,
                ShippingMethod = shippingMethod,
                Items = items,
                ShippingAddress = shippingAddress
            };
        }

        [Theory]
        [InlineData("country", "city")]
        [InlineData("country", "different city")]
        [InlineData("different country", "city")]
        public void CalculateShippingCost_StandardShippingNoItems_Return0(string country, string city)
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                items: new List<Item> { },
                shippingAddress: new Address { City = city, Country = country, Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_SameCityStandardShippingOneItemsDifferentQuantities_ReturnQuantityTimesRate(uint Quantity)
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                items: new List<Item> { new Item { Quantity = Quantity } },
                shippingAddress: new Address { City = "city", Country = "country", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(Quantity * ShippingCalculator.SameCityRate, result);
        }

        [Theory]
        [InlineData("country", "city", ShippingCalculator.SameCityRate)]
        [InlineData("country", "different city", ShippingCalculator.SameCountryRate)]
        [InlineData("different country", "city", ShippingCalculator.InternationalShippingRate)]
        public void CalculateShippingCost_StandardShippingTwoItems_ReturnSumOfItemsQuantityTimesRate(string country, string city, double rate)
        {
            var address = new Address { Country = "country", City = "city", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                items: new List<Item>
                {
                    new() { Quantity = 5 },
                    new() { Quantity = 3 }
                },
                shippingAddress: new Address { Country = country, City = city, Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(8 * rate, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_SameCountryStandardShippingOneItemsDifferentQuantities_ReturnQuantityTimesRate(uint Quantity)
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                items: new List<Item> { new Item { Quantity = Quantity } },
                shippingAddress: new Address { Country = "country", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(Quantity * ShippingCalculator.SameCountryRate, result);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void CalculateShippingCost_InternationalShippingStandardShippingOneItemsDifferentQuantities_returnQuantityTimesRate(uint Quantity)
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                items: new List<Item> { new Item { Quantity = Quantity } },
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(Quantity * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Expedited,
                shippingAddress: new Address { City = "city", Country = "country", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpeditedShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Expedited,
                shippingAddress: new Address { Country = "country", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Expedited,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 1.2, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Priority,
                shippingAddress: new Address { City = "city", Country = "country", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryPriorityShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Priority,
                shippingAddress: new Address { Country = "country", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Priority,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.0, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCityExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { City = "city", Country = "country", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Express,
                shippingAddress: new Address { City = "city", Country = "country", Street = "street 2" });

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCityRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_SameCountryExpressShippingOneItemsQuantity1_Return1TimesRate()
        {
            var address = new Address { Country = "country", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Express,
                shippingAddress: new Address { Country = "country", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.SameCountryRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                shippingMethod: ShippingMethod.Express,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingPriorityShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                customerType: CustomerType.Premium,
                shippingMethod: ShippingMethod.Priority,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpeditedShippingPremiumCustomerOneItemsQuantity1_DoNotPayShippingRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                customerType: CustomerType.Premium,
                shippingMethod: ShippingMethod.Expedited,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate, result);
        }

        [Fact]
        public void CalculateShippingCost_InternationalShippingExpressShippingVIPCustomerOneItemsQuantity1_return1TimesRate()
        {
            var address = new Address { Country = "country 1", City = "city 1", Street = "street 1" };

            var target = new ShippingCalculator(address);

            var cart = CreateCart(
                customerType: CustomerType.Standard,
                shippingMethod: ShippingMethod.Express,
                shippingAddress: new Address { Country = "country 2", City = "city 2", Street = "street 2" });
            var result = target.CalculateShippingCost(cart);

            Assert.Equal(1 * ShippingCalculator.InternationalShippingRate * 2.5, result);
        }
    }
}