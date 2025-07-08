using E_Ticaret_Project.Application.Abstracts.Services;
using E_Ticaret_Project.Application.DTOs.OrderDtos;
using E_Ticaret_Project.Application.DTOs.ProductDtos;
using E_Ticaret_Project.Application.Shared.Responses;
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
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result=await _orderService.GetAllAsync();
            return StatusCode((int)result.StatusCode,result);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<OrderGetDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMyOrdersAsync()
        {
            var result = await _orderService.GetMyOrdersAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<OrderGetDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetMySalesAsync()
        {
            var result = await _orderService.GetMySalesAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        // POST api/<OrdersController>
        [HttpPost]
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
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelOrderAsync(Guid id)
        {
            var result = await _orderService.CancelOrderAsync(id);
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
