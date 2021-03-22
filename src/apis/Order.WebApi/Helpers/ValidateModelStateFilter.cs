using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Order.WebApi.Helpers
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelErrors = context.ModelState.Keys
                     .SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage))
                     .ToList();
                context.HttpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                context.Result = new JsonResult(new
                {
                    ErrMsg = "Model Validation Failed",
                    Detail = modelErrors
                });
            }
        }
    }
}
