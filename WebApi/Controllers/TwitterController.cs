using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : Controller
    {
        private readonly IMediator _mediator;

        public TwitterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("start-stream")]
        public async Task<IActionResult> StartStream()
        {
            var response = await _mediator.Send(new StartStream.Command());
            return Ok(response);
        }

    }
}
