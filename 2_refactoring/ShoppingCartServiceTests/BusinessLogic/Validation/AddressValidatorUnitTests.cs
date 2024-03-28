using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartServiceTests.BusinessLogic.Validation
{
    public class AddressValidatorUnitTests
    {
        [Theory]
        [InlineData(null, "city-1", "1234 street")]
        [InlineData("country-1", null, "1234 street")]
        [InlineData("country-1", "city-1", null)]
        public void IsValid_DoesHaveNullAtrribute_returnFalse(string country, string city, string street)
        {
            var address = new Address
            {
                Country = country,
                City = city,
                Street = street
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.False(result);
        }


        [Fact]
        public void IsValid_validValues_returnTrue()
        {
            var address = new Address
            {
                Country = "country-1",
                City = "city-1",
                Street = "street-1"
            };

            var target = new AddressValidator();

            var result = target.IsValid(address);

            Assert.True(result);
        }
    }
}
