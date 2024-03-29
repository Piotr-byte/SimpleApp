﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleApp.Core;
using SimpleApp.Core.Models.Entities;
using SimpleApp.WebApi.DTO;
using Xunit;

namespace SimpleApp.WebApi.UnitTests.Controllers.Categories
{
    public class GetTests : BaseTests
    {
        [Fact]
        public async Task Return_BadRequest_When_Categories_Is_Not_Valid()
        {
            // Arrange
            var controller = Create();
            var errorMessage = "BadRequest";
            var categories = Builder<Category>.CreateListOfSize(1).Build().AsEnumerable();
            CategoryLogicMock.Setup(x => x.GetAllActiveAsync())
                .ReturnsAsync(Result.Failure<IEnumerable<Category>>(errorMessage));

            // Act
            var result = await controller.GetAsync();

            // Assert
            result.Should().BeBadRequest<IEnumerable<Category>>(errorMessage);
            CategoryLogicMock.Verify(
                x => x.GetAllActiveAsync(), Times.Once());

            MapperMock.Verify(
                x => x.Map<IList<CategoryDto>>(It.IsAny<IEnumerable<Category>>()), Times.Never());
        }

        [Fact]
        public async Task Return_All_Categories()
        {
            // Arrange
            var controller = Create();
            var categories = Builder<Category>.CreateListOfSize(1).Build().AsEnumerable();
            var categoryDtos = Builder<CategoryDto>.CreateListOfSize(1).Build();
            CategoryLogicMock
                .Setup(r => r.GetAllActiveAsync())
                .ReturnsAsync(Result.Ok(categories));
            MapperMock
                .Setup(m => m.Map<IList<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
                .Returns(categoryDtos);

            // Act
            var result = await controller.GetAsync();

            // Assert
            result.Should().BeOk(categoryDtos);
            CategoryLogicMock.Verify(
                x => x.GetAllActiveAsync(), Times.Once());

            MapperMock.Verify(
                x => x.Map<IList<CategoryDto>>(categories), Times.Once());
        }
    }
}
