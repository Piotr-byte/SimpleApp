﻿using System;
using System.Threading;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using SimpleApp.Core;
using SimpleApp.Core.Models.Entities;
using Xunit;

namespace SimpleApp.Core.UnitTests.Logic.Orders
{
    public class UpdateTests : BaseTests
    {
        [Fact]
        public void Throw_ArgumentNullException_When_Argument_Is_Null()
        {
            // Arrange
            var logic = Create();

            // Act
            Func<Task> result = async () => await logic.UpdateAsync(null);

            // Assert
            result.Should().Throw<ArgumentNullException>();
            ValidatorMock.Verify(
                x => x.ValidateAsync(It.IsAny<Order>(), CancellationToken.None), Times.Never());

            OrderRepositoryMock.Verify(
                x => x.SaveChangesAsync(), Times.Never());
        }

        [Fact]
        public async Task Return_Failure_When_Order_Is_Not_Valid()
        {
            // Arrange
            var logic = Create();
            var order = Builder<Order>.CreateNew().Build();
            const string errorMessage = "validation fail";
            ValidatorMock.SetValidationFailure(order.Id.ToString(), errorMessage);

            // Act
            var result = await logic.UpdateAsync(order);

            // Assert
            result.Should().BeFailure(property: order.Id.ToString(), message: errorMessage);
            ValidatorMock.Verify(
                x => x.ValidateAsync(order, CancellationToken.None), Times.Once());

            OrderRepositoryMock.Verify(
                x => x.SaveChangesAsync(), Times.Never());
        }

        [Fact]
        public async Task Return_Success_When_Order_Is_Valid()
        {
            // Arrange
            var logic = Create();
            var order = Builder<Order>.CreateNew().Build();
            ValidatorMock.SetValidationSuccess();

            // Act
            var result = await logic.UpdateAsync(order);

            // Assert
            result.Should().BeSuccess(order);
            ValidatorMock.Verify(
                x => x.ValidateAsync(order, CancellationToken.None), Times.Once());

            OrderRepositoryMock.Verify(
                x => x.SaveChangesAsync(), Times.Once());
        }
    }
}
