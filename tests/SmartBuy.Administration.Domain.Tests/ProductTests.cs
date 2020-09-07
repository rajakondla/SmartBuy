using System;
using Moq;
using Xunit;

namespace SmartBuy.Administration.Domain.Tests
{
    public class ProductTests
    {
        private string _productName;

        public ProductTests()
        {
            _productName = "P1";
        }

        [Fact]
        public void ShouldMatchProductInformationWithAssignedValues()
        {
            var id = 1;
            var product = new Product(id);
            product.Name = _productName;

            Assert.Equal(id, product.Id);
            Assert.Equal(_productName, product.Name);
        }
    }
}
