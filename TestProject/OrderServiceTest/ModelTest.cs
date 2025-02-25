using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;


namespace TestProject.OrderServiceTest
{
    public class MyWebApplicationContextTests
    {
        private DbContextOptions<MyWebApplicationContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<MyWebApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void CanAddAndRetrieveOrder()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new MyWebApplicationContext(options))
            {
                var order = new Order
                {
                    OrderId = 1,
                    ProductId = "P001",
                    Quantity = 10,
                    CustomerName = "John Doe",
                    Address = "123 Main St"
                };

                // Act
                context.Orders.Add(order);
                context.SaveChanges();
            }

            // Assert
            using (var context = new MyWebApplicationContext(options))
            {
                var retrievedOrder = context.Orders.Find(1);
                Assert.NotNull(retrievedOrder);
                Assert.Equal("P001", retrievedOrder.ProductId);
                Assert.Equal(10, retrievedOrder.Quantity);
                Assert.Equal("John Doe", retrievedOrder.CustomerName);
                Assert.Equal("123 Main St", retrievedOrder.Address);
            }
        }
    }

    public class OrderTests
    {
        [Fact]
        public void TestOrderInitialization()
        {
            // Arrange
            var order = new Order();

            // Act
            // (No action needed for initialization test)

            // Assert
            Assert.NotNull(order);
        }

        [Fact]
        public void TestOrderProperties()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                ProductId = "TestOrder"
            };

            // Act
            // (No action needed for property test)

            // Assert
            Assert.Equal(1, order.OrderId);
            Assert.Equal("TestOrder", order.ProductId);
        }
    }
}
