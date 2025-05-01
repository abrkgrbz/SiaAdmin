using System.Net;
using System.Text.Json;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.WebUI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                List<string> message = new List<string>();
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() { Succeeded = false, Message = error?.Message };

                switch (error)
                {
                    case Application.Exceptions.ApiException e:
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        // custom application error
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UnauthorizedAccessException e:
                        // unauthorized access error
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    case NotImplementedException e:
                        // not implemented error
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.NotImplemented;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }

    }
}
