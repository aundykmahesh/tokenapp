using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseController : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator() => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result){
            if (result==null)
            {
                return NotFound();
            }
            if (result.isSuccess && result.Value!=null)
            {
                return Ok(result);
            }
            if (result.isSuccess && result.Value==null)
            {
                return NotFound();
            }
            return BadRequest(result.Error);
        }

    }
}