using Microsoft.AspNetCore.Http;
using RedisServer.Helper;
using RedisServer.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RedisServer.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly IRedisLogger _ReddisLogger;

        public ErrorHandlerMiddleware(RequestDelegate Next,
            IRedisLogger RedisLogger)
        {
            _ReddisLogger = RedisLogger;
            _Next = Next;
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await _Next(Context);
            }
            catch (Exception Error)
            {
                var Response = Context.Response;
                Response.ContentType = "application/json";

                switch (Error)
                {
                    case AppException e:
                        // custom application error
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break; 
                    case InvalidOperationException e:
                        // not found error
                        Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                        break;
                    default:
                        // unhandled error
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                _ReddisLogger.Error("API returned an exception {0}", Error?.Message);
                await Response.WriteAsync(Error?.Message);
            }
        }
    }
}