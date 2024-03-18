using MISA.FRESHER032023.COMMON.Exceptions;
using System.Net;

namespace MISA.FresherWeb032023.Controllers.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }
        /// <summary>
        /// Xử lí thông báo lỗi trả về
        /// </summary>
        /// <param name="context">request</param>
        /// <param name="exception">Exception</param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            // Kiểm tra type của exception
            if (exception is NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;


                await context.Response.WriteAsync(
                   text: new BaseException()
                   {
                       ErrorCode = ((NotFoundException)exception).ErrorCode,
                       UserMessage = ((NotFoundException)exception).Message,
                       DevMsg = exception.Message,
                       TraceId = context.TraceIdentifier,
                       MoreInfo = exception.HelpLink
                   }.ToString() ?? ""
                 );

            }
            else if (exception is InternalException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(
                   text: new BaseException()
                   {
                       ErrorCode = context.Response.StatusCode,
                       UserMessage = exception.Message,
                       DevMsg = exception.Message,
                       TraceId = context.TraceIdentifier,
                       MoreInfo = exception.HelpLink
                   }.ToString() ?? ""
                 );
            }
        }
    }

}
