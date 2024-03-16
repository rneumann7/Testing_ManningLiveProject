using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartService.Tests
{
    public class ShoppingCartService_AddressValidation
    {
        [Fact]
        public void IsValid_ValidAddress_ReturnTrue()
        {
            var addressValidator = new AddressValidator();

            bool result = addressValidator.IsValid(new Address
            {
                Street = "Street",
                City = "City",
                Country = "Country"
            });

            Assert.True(result, "Valid address should return true");
        }

        [InlineData("")]
        [InlineData(null)]
        [Theory]
        public void IsValid_StreetIsNullOrEmpty_ReturnFalse(string street)
        {
            var addressValidator = new AddressValidator();
            bool result = addressValidator.IsValid(new Address
            {
                Street = street,
                City = "City",
                Country = "Country"
            });

            Assert.False(result, "Street empty or null should return false");
        }

        [InlineData("")]
        [InlineData(null)]
        [Theory]
        public void IsValid_CountryIsNullOrEmpty_ReturnFalse(string country)
        {
            var addressValidator = new AddressValidator();
            bool result = addressValidator.IsValid(new Address
            {
                Street = "Street",
                City = "City",
                Country = country
            });

            Assert.False(result, "Country empty or null should return false");
        }

        [InlineData("")]
        [InlineData(null)]
        [Theory]
        public void IsValid_CityIsNullOrEmpty_ReturnFalse(string city)
        {
            var addressValidator = new AddressValidator();
            bool result = addressValidator.IsValid(new Address
            {
                Street = "Street",
                City = city,
                Country = "Country"
            });

            Assert.False(result, "City empty or null should return false");
        }
    }
}