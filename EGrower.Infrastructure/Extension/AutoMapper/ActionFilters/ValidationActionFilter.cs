using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EGrower.Infrastructure.Extension.AutoMapper.ActionFilters {
    public class ValidationActionFilter {
        public class ValidatorActionFilter : IActionFilter {
            public void OnActionExecuting (ActionExecutingContext filterContext) {
                if (!filterContext.ModelState.IsValid) {
                    filterContext.Result = new BadRequestObjectResult (filterContext.ModelState);
                }
            }
            public void OnActionExecuted (ActionExecutedContext filterContext) { }
        }
    }
}