using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Ticaret_Project.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrderService _orderService { get; }
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/<OrdersController>
        [HttpGet]
        [Authorize(Policy = "Order.GetAll")]
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result=await _orderService.GetAllAsync();
            return StatusCode((int)result.StatusCode,result);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "Order.GetDetail")]
        [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Policy = "Order.GetMy")]
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMyOrdersAsync([FromQuery] string userId)
        {
            var result = await _orderService.GetMyOrdersAsync(userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [Authorize(Policy = "Order.MySales")]
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMySalesAsync()
        {
            var result = await _orderService.GetMySalesAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        // POST api/<OrdersController>
        [HttpPost]
        [Authorize(Policy = "Order.Create")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var result = await _orderService.CreateOrder(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "Order.Delete")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelOrderAsync(Guid id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id},{orderStatus}")]
        [Authorize(Policy = "Order.Update")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateOrderAsync(Guid id, string orderStatus)
        {
            var result = await _orderService.UpdateOrderAsync(id,orderStatus);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ConfirmOrder(Guid orderId)
        {
            var result = await _orderService.ConfirmOrder(orderId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
