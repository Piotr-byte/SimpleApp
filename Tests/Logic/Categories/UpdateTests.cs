﻿using System;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using SimpleApp.Core;
using SimpleApp.Core.Models.Entity;
using Xunit;

namespace SimpleApp.Core.UnitTests.Logic.Categories
{
    public class UpdateTests : BaseTests
    {
        [Fact]
        public void Throw_ArgumentNullException_When_Argument_Is_Null()
        {
            // Arrange
            var logic = Create();

            // Act
            Action result = () => logic.Update(null);

            // Assert
            result.Should().Throw<ArgumentNullException>();
            ValidatorMock.Verify(
                x => x.Validate(It.IsAny<Category>()), Times.Never());

            CategoryRepositoryMock.Verify(
                x => x.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Return_Failure_When_Category_Is_Not_Valid()
        {
            // Arrange
            var logic = Create();
            var category = Builder<Category>.CreateNew().Build();
            const string errorMessage = "validation fail";
            ValidatorMock.SetValidationFailure(category.Name, errorMessage);

            // Act
            var result = logic.Update(category);

            // Assert
            result.Should().BeFailure(property: category.Name, message: errorMessage);
            ValidatorMock.Verify(
                x => x.Validate(category), Times.Once());

            CategoryRepositoryMock.Verify(
                x => x.SaveChanges(), Times.Never());
        }

        [Fact]
        public void Return_Success_When_Category_Is_Valid()
        {
            // Arrange
            var logic = Create();
            var category = Builder<Category>.CreateNew().Build();
            ValidatorMock.SetValidationSuccess();

            // Act
            var result = logic.Update(category);

            // Assert
            result.Should().BeSuccess(category);
            ValidatorMock.Verify(
                x => x.Validate(category), Times.Once());

            CategoryRepositoryMock.Verify(
                x => x.SaveChanges(), Times.Once());
        }
    }
}
