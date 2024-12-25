using DatPhongNhanh.WebApi.Filters;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DatPhongNhanh.WebApi.Controllers
{
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public abstract class ApiController : ControllerBase
    {
        private ISender? mediator;
        protected ISender Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected ActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors[0]);
        }

        private ObjectResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };
            return Problem(statusCode: statusCode, title: error.Description, instance: Request.Path, detail: string.Empty);
        }


        private ActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            errors.ForEach(error => modelStateDictionary.AddModelError(error.Code, error.Description));

            return ValidationProblem(modelStateDictionary: modelStateDictionary, instance: Request.Path, detail: string.Empty);
        }

    }
}
