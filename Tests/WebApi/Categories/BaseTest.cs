﻿using AutoMapper;
using Moq;
using SimpleApp.Core.Interfaces.Logics;
using SimpleApp.WebApi.Controllers;

namespace SimpleApp.Core.UnitTests.WebApi.Categories
{
    public class BaseTest
    {
        protected Mock<ICategoryLogic> CategoryLogicMock { get; set; }
        protected Mock<IMapper> MapperMock { get; set; }
        protected CategoryController Create()
        {
            CategoryLogicMock = new Mock<ICategoryLogic>();
            MapperMock = new Mock<IMapper>();
            return new CategoryController (
                CategoryLogicMock.Object,
                MapperMock.Object
                );
        }
    }
}
