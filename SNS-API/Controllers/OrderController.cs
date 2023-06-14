﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNS_API.Controllers.BaseController;
using SNS_BLA.Services.OrderService;
using SNS_BLA.Services.ProductService;
using SNS_BLA.Services.UserService;
using SNS_DLA.Models.DTO_s.Request;
using SNS_DLA.Models.DTO_s.Response;
using SNS_DLA.Models.Entities;
using System.Text.Json;

namespace SNS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "CustomerOrEmployee")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService service, IUserService userService)
        {
            _orderService = service;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Cart")]
        public async Task<ActionResult<CartResponse>> GetCart([FromBody] List<CartItemRequest> cartItemsRequest) 
        {
            try
            {
                var cartResponse = await _orderService.GetCart(cartItemsRequest);

                if (!cartResponse.CartItems.Any())
                {
                    return NotFound();
                }

                return Ok(cartResponse);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<ActionResult> PlaceOrder(OrderRequest orderRequest)
        {
            try
            {
                var emailClaim = User.Claims.FirstOrDefault(c => c.Type == "Email");
                var orderInfo = new OrderInfo();

                if (emailClaim == null)
                {
                    return BadRequest();
                }

                Console.WriteLine(emailClaim.Value);
                orderInfo.UserEmail = emailClaim.Value;
                orderInfo.OrderedProducts = orderRequest.OrderedProducts;
                orderInfo.TotalPrice = orderRequest.TotalPrice;

                var success = await _orderService.PlaceOrder(orderInfo);

                if (success)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Authorize(Policy = "EmployeeOnly")]
        public async Task<ActionResult<OrderResponse>> GetAllOrderAsync()
        {
            try
            {
                var allOrders = await _orderService.GetOrdersAsync();

                if (allOrders == null)
                {
                    return NotFound();
                }

                return Ok(allOrders);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Authorize(Policy = "EmployeeOnly")]
        [Route("{id}")]
        public async Task<ActionResult> DeleteOrderAsync([FromRoute] int id)
        {
            try
            {
                var result = await _orderService.DeleteAsync(id);

                if (result)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
