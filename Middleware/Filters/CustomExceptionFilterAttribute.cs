using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace loconotes.Middleware.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
	    public override void OnException(ExceptionContext context)
	    {
			var customExceptionHandlerProvider = new Lazy<CustomExceptionHandlerProvider>();

			var customExceptionHandler = customExceptionHandlerProvider.Value.CustomExceptionsToHandle.FirstOrDefault(x => x.ExceptionType == context.Exception.GetType());

		    if (customExceptionHandler != null)
		    {
			    if (context.Exception.Message.Contains("Exception of type"))
			    {
				    context.Result = customExceptionHandler.StatusCodeResult;
			    }
			    else
			    {
				    context.Result = new ContentResult
				    {
					    Content = context.Exception.Message,
					    ContentType = "text/plain",
					    StatusCode = (int?) customExceptionHandler.StatusCode
				    };
			    }
		    }

		    base.OnException(context);
	    }
    }

	public class CustomExceptionHandlerProvider
	{
		public CustomExceptionHandlerProvider()
		{
			CustomExceptionsToHandle = new[]
			{
				new CustomExceptionToHandle
				{
					ExceptionType = typeof(NotFoundException),
					StatusCode = HttpStatusCode.NotFound,
					StatusCodeResult = new NotFoundResult()
				},
				new CustomExceptionToHandle
				{
					ExceptionType = typeof(ValidationException),
					StatusCode = HttpStatusCode.BadRequest,
					StatusCodeResult = new BadRequestResult()
				},
				new CustomExceptionToHandle
				{
					ExceptionType = typeof(UnauthorizedAccessException),
					StatusCode = HttpStatusCode.Unauthorized,
					StatusCodeResult = new UnauthorizedResult()
				},
				//new CustomExceptionToHandle
				//{
				//	ExceptionType = typeof(ConflictException),
				//	StatusCode = HttpStatusCode.Conflict,
				//	StatusCodeResult = new StatusCodeResult((int)HttpStatusCode.Conflict),
				//}
			};
		}

		public CustomExceptionToHandle[] CustomExceptionsToHandle { get; set; }
	}


	public class CustomExceptionToHandle
	{
		public Type ExceptionType { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public StatusCodeResult StatusCodeResult { get; set; }
	}
}
