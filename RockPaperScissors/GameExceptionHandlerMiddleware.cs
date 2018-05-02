using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RockPaperScissors
{
    public class GameExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _logger;

        public GameExceptionHandlerMiddleware(RequestDelegate next,
                                              ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                .CreateLogger<GameExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(GameException e)
            {
                await HandleGameExceptionAsync(context, e);
            }
        }

        private Task HandleGameExceptionAsync(HttpContext context, GameException e)
        {

            int statusCode = 500;
            string message = String.Empty;

            switch(e)
            {
            case GameNotFoundException err:
                statusCode = 404;
                break;

            default:
                statusCode = 400;
                _logger.LogInformation($"Caught GameException: {e.Message}");
                break;
            }

            var errResult = new { message = e.Message };
            var jsonResult = JsonConvert.SerializeObject(errResult);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonResult);
        }
    }
}
