using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace PlagiarismCore.Extensions
{
    public class GlobalErrorHandlerAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is CustomException customException)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/_CustomError.cshtml",
                    ViewData = new ViewDataDictionary(new ErrorModel
                    {
                        Controller = filterContext.RouteData.Values["controller"]?.ToString(),
                        Action = filterContext.RouteData.Values["action"]?.ToString(),
                        Errors = customException.Errors
                    })
                };
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/_CustomError.cshtml",
                    ViewData = new ViewDataDictionary(new ErrorModel
                    {
                        Controller = filterContext.RouteData.Values["controller"]?.ToString(),
                        Action = filterContext.RouteData.Values["action"]?.ToString(),
                        Errors = new List<string>
                        {
                           filterContext?.Exception?.Message?.ToString(),
                           filterContext?.Exception?.InnerException?.Message?.ToString()
                        }
                    })
                };
            }
            filterContext.ExceptionHandled = true;
        }
    }
}