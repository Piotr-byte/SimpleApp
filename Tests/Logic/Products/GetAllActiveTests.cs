﻿using FizzWare.NBuilder;
using Moq;
using SimpleApp.Core.Models;
using Xunit;

namespace SimpleApp.Core.UnitTests.Logic.Products
{
    public class GetAllActiveTests : BaseTests
    {
        [Fact]
        public void Return_All_Products_From_Repository()
        {
            //Arrange
            var logic = Create();
            var products = Builder<Product>.CreateListOfSize(10).Build();
            ProductRespositoryMock
                .Setup(r => r.GetAllActive()).Returns(products);

            //Act
            var result = logic.GetAllActive();

            //Assert
            result.Should().BeSuccess(products);
            ProductRespositoryMock.Verify(
                x => x.GetAllActive(), Times.Once());
        }
    }
}