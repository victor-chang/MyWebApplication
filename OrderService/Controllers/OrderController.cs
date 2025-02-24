using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KafkaService;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderService.Models;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly KafkaProducerService _kafkaProducerService;
        private readonly MyWebApplicationContext _dbConntext;
        public OrderController(ILogger<WeatherForecastController> logger, KafkaProducerService kafkaProducerService, MyWebApplicationContext dbContext)
        {
            _logger = logger;
            _kafkaProducerService = kafkaProducerService;
            _dbConntext = dbContext;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Order is null");
                
            }

            try
            {
                var orderId = 1;
                if (_dbConntext.Orders.Count() > 0)
                {
                    orderId = _dbConntext.Orders.Max(o => o.OrderId) + 1;
                }

                order.OrderId = orderId;

                var orderEntry = _dbConntext.Orders.Add(order);
                await _dbConntext.SaveChangesAsync();

                // Send order to Kafka
                //await _kafkaProducerService.SendMessageAsync("order-topic", JsonSerializer.Serialize(order));

                return Ok(order.OrderId);
            }

            catch (Exception ex)
            {
                // log error
                _logger.LogError(ex, "Error creating order");
                // return error message
                return BadRequest("Something wrong");
            }

            

        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrder(int orderID)
        {
            var o = await _dbConntext.Orders.FirstOrDefaultAsync(o => o.OrderId == orderID);
            if (o == null)
            {
                return BadRequest("OrderId does not exist.");
            }
            else
            {
                return Ok(o);
            }
        }

       



    }
}
