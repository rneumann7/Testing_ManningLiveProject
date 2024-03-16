using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using static ShoppingCartService.Tests.Helper;
using AutoMapper;
using ShoppingCartService.Mapping;
using Xunit;

namespace ShoppingCartService.Tests
{
    public class ShoppingCartService_CheckoutEngine
    {

        [Fact]
        public void CalculateTotals_NoDiscount_ReturnsCheckoutDto()
        {
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Standard, CustomerType.Standard, 3);
            var shippingCalculator = new ShippingCalculator();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            var checkoutEngine = new CheckOutEngine(shippingCalculator, mapper);

            var result = checkoutEngine.CalculateTotals(cart);

            Assert.Equal(33, result.Total);
        }

        [Fact]
        public void CalculateTotals_Discount_ReturnsCheckoutDto()
        {
            var cart = CreateCart("USA", "Dallas", ShippingMethod.Standard, CustomerType.Premium, 3);
            var shippingCalculator = new ShippingCalculator();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            var checkoutEngine = new CheckOutEngine(shippingCalculator, mapper);
            var expectedTotal = 33 * 0.9;

            var result = checkoutEngine.CalculateTotals(cart);

            Assert.Equal(expectedTotal, result.Total);
        }
    }
}