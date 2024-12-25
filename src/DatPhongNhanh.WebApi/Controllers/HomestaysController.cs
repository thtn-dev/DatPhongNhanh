using DatPhongNhanh.Application.Homestay.Commands;
using DatPhongNhanh.Application.Homestay.Queries;
using DatPhongNhanh.Domain.Homestay.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatPhongNhanh.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomestaysController : ApiController
    {
        private readonly IHomestayService _homestayService;
        public HomestaysController(IHomestayService homestayService)
        {
            _homestayService = homestayService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHomestayCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetHomestayByIdQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("fi")]
        public async Task<IActionResult> GetNearest([FromQuery] Query q)
        {
            var r = await _homestayService.FindHomestayNearest(q.Longitude, q.Latitude, q.Tolerance);
            return Ok(r);
        }

        public class Query
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public double Tolerance { get; set; }
        }
    }
}
