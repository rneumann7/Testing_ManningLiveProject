using ShoppingCartService.BusinessLogic;
using ShoppingCartService.Models;
using static ShoppingCartService.Tests.Helper;
using Xunit;

namespace ShoppingCartService.Tests
{

    public class ShoppingCartService_ShippingCaclulator
    {
        [Fact]
        public void CalculateShippingCost_TravelCostSameCountry_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Houston", ShippingMethod.Standard, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(6, result);
        }

        [Fact]
        public void CalculateShippingCost_TravelCostSameCity_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Standard, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(3, result);
        }

        [Fact]
        public void CalculateShippingCost_TravelCostInternational_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("Germany", "Berlin", ShippingMethod.Standard, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(45, result);
        }

        [Fact]
        public void CalculateShippingCost_StandardCustomerStandardShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Standard, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(3, result);
        }

        [Fact]
        public void CalculateShippingCost_StandardCustomerExpeditedShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            uint numberOfItems = 3;
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Expedited, CustomerType.Standard, numberOfItems);
            var expected = numberOfItems * 1.2;

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateShippingCost_StandardCustomerPriorityShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Priority, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(6, result);
        }

        [Fact]
        public void CalculateShippingCost_StandardCustomerExpressShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Express, CustomerType.Standard, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(7.5, result);
        }

        [Fact]
        public void CalculateShippingCost_PremiumCustomerStandardShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Standard, CustomerType.Premium, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(3, result);
        }

        [InlineData(ShippingMethod.Expedited)]
        [InlineData(ShippingMethod.Priority)]
        [Theory]
        public void CalculateShippingCost_PremiumCustomerExpeditedOrPriorityShipping_ReturnsShippingCost(ShippingMethod shippingMethod)
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", shippingMethod, CustomerType.Premium, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(3, result);
        }

        [Fact]
        public void CalculateShippingCost_PremiumCustomerExpressShipping_ReturnsShippingCost()
        {
            var shippingCalculator = new ShippingCalculator();
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Express, CustomerType.Premium, 3);

            var result = shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(7.5, result);
        }

    }
}