using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderService.Controllers; // Assuming the namespace for OrderController
using OrderService.Models; // Assuming the namespace for Order model
using Microsoft.AspNetCore.Mvc;
using KafkaService;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using NuGet.ContentModel;
using Namotion.Reflection;

namespace TestProject.OrderServiceTest
{


    public class OrderControllerTest
    {
        private readonly Mock<ILogger<OrderController>> _mockLogger;
        private readonly KafkaConfig _mockKafkaConfig;
        private readonly Mock<KafkaProducerService> _mockKafkaProducerService;
        private static MyWebApplicationContext? _dbContext; // in memory db context
        private readonly OrderController _controller;
        
        public OrderControllerTest()
        {
            // Create mock objects for dependencies
            _mockLogger = new Mock<ILogger<OrderController>>();
            _mockKafkaConfig = new KafkaConfig("", "");
            _mockKafkaProducerService = new Mock<KafkaProducerService>(_mockKafkaConfig);

            // setup in memory db context
            if (_dbContext == null)
            {
                var options = new DbContextOptionsBuilder<MyWebApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;


                _dbContext = new MyWebApplicationContext(options);


                // seed data
                _dbContext.Orders.AddRange(

                    new Order { OrderId = 1, ProductId = "P001", Quantity = 10, CustomerName = "John Doe", Address = "123 Main St" },
                    new Order { OrderId = 2, ProductId = "P011", Quantity = 1, CustomerName = "Mary Harris", Address = "456 Highland St" }
                );

                _dbContext.SaveChanges();
            }

            // Initialize OrderController with mock dependencies
            _controller = new OrderController(_mockLogger.Object, _mockKafkaProducerService.Object, _dbContext);
        }



        [Fact]
        public async Task GetOrderById_ReturnsOkResult()
        {
            // Arrange
            var testOrderId = 1;
          
            // Act
            var result = await _controller.GetOrder(testOrderId);

            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, (okResult.Value as Order).OrderId);
            Assert.Equal("P001", (okResult.Value as Order).ProductId);
            Assert.Equal(10, (okResult.Value as Order).Quantity);
            Assert.Equal("John Doe", (okResult.Value as Order).CustomerName);
            Assert.Equal("123 Main St", (okResult.Value as Order).Address);

        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFoundResult()
        {
            // Arrange
            var testOrderId = -1;

            // Act
            var result = await _controller.GetOrder(testOrderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult()
        {

            // Arrange
            var newOrder = new Order {
                OrderId = 0,
                ProductId = "P001",
                Quantity = 10,
                CustomerName = "John Doe",
                Address = "123 Main St"
            };

            // Act
            var result = await _controller.CreateOrder(newOrder);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotEqual(0, okResult.Value);
        }
    }


}