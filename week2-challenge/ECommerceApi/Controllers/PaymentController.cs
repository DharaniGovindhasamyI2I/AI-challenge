using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Data;
using Microsoft.AspNetCore.Authorization;
using Stripe;
using ECommerceApi.Models.Payment;
using ECommerceApi.Models.Payment.Strategies;
using ECommerceApi.Models;
using ECommerceApi.Services;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public IActionResult Pay([FromBody] PaymentRequest request)
        {
            // Simulate payment logic
            bool isSuccess = true; // or false
            _paymentService.ProcessPayment(request.Email, request.Product, request.Price, isSuccess);
            return Ok(new { status = isSuccess ? "success" : "failure" });
        }
    }

    public class PaymentRequest
    {
        public string Email { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
}