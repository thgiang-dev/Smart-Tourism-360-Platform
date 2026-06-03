using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Common.Models;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = "Request completed successfully.", int statusCode = 200)
        {
            return StatusCode(statusCode, ApiResponse<T>.SuccessResponse(data, message));
        }

        protected IActionResult Failure(string message, List<ValidationError>? errors = null, int statusCode = 400)
        {
            return StatusCode(statusCode, ApiResponse<object>.FailureResponse(message, errors));
        }
    }
}
