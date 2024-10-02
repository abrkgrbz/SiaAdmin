using System.Net;
using System.Text.Json;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http; 
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.API.Middlewares
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
                var responseModel = new Response<string>() { Succeeded = false };
                var type = error.GetType();
                switch (error)
                {
                    case Application.Exceptions.ApiException e: 
                        message.Clear();
                        message.Add(e.Message);
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException e:
                        message.Clear();
                        message.Add(e.Message);
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = e.Errors;
                        break;
                    case KeyNotFoundException e:
                        message.Clear();
                        message.Add(e.Message);
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case FirebaseAuthException e:
                        if (e.AuthErrorCode == AuthErrorCode.ExpiredIdToken)
                        { 
                            message.Add("Sağlanan Firebase kimliği jetonunun süresi dolmuş!");
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            responseModel.Errors = message;
                            break;
                        }

                        if (e.AuthErrorCode==AuthErrorCode.InvalidIdToken)
                        {
                            message.Add("Sağlanan kimlik jetonu geçerli bir Firebase kimliği jetonu değil");
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            responseModel.Errors = message;
                            break;
                        }
                        message.Clear();
                        message.Add(e.Message);
                        response.StatusCode=(int)HttpStatusCode.BadRequest;
                        responseModel.Errors = message;
                        break;
                    case UnauthorizedAccessException e:
                        message.Clear();
                        message.Add("Kimlik doğrulaması gerekmekte!");
                        responseModel.Errors = message;
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:

                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }
}
