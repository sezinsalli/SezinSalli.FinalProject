﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simpra.Core.Entity;
using Simpra.Core.Service;
using Simpra.Schema.CategoryRR;
using Simpra.Schema.OrderRR;
using Simpra.Service.Exceptions;
using Simpra.Service.Reponse;
using Simpra.Service.Service.Abstract;

namespace Simpra.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _service;

        public OrderController(IMapper mapper, IOrderService service)
        {
            _service = service;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var orders = _service.GetOrdersWithOrderDetails();

            var orderResponse = _mapper.Map<List<OrderResponse>>(orders);

            return Ok(CustomResponse<List<OrderResponse>>.Success(200, orderResponse));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //Include
            var order = await _service.GetByIdAsync(id);

            if (order == null)
            {
                return CreateActionResult(CustomResponse<OrderResponse>.Fail(400, "Bu id'ye sahip ürün bulunmamaktadır."));
            }

            var orderResponse = _mapper.Map<OrderResponse>(order);

            return CreateActionResult(CustomResponse<OrderResponse>.Success(200, orderResponse));
        }

        /*
         1. Coupon => 
            a. Coupon aktif mi? Expiration Date kontrol => is active false => Coupon update
            b. UserId ve coupon check edeceğiz

         2. Points => 
            a. 
         
         
         
         
         
         */


        [HttpPost]
        public async Task<IActionResult> Save(OrderCreateRequest orderCreateRequest)
        {
            var order = _mapper.Map<Order>(orderCreateRequest);

            var orderResult = await _service.CreateOrderAsync(order);

            var orderResponse = _mapper.Map<OrderResponse>(orderResult);

            return CreateActionResult(CustomResponse<OrderResponse>.Success(201, orderResponse));
        }

        [HttpPut]
        public async Task<IActionResult> Update(OrderUpdateRequest orderUpdateRequest)
        {
            await _service.UpdateAsync(_mapper.Map<Order>(orderUpdateRequest));

            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var order = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(order);
            return CreateActionResult(CustomResponse<NoContent>.Success(204));
        }
    }
}
