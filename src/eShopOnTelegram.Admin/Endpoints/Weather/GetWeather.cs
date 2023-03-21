using Ardalis.ApiEndpoints;

using eShopOnTelegram.Admin.Constants;
using eShopOnTelegram.Admin.Extensions;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace eShopOnTelegram.Admin.Weather.Endpoints
{
    public class GetWeather : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<IEnumerable<WeatherForecast>>
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<GetWeather> _logger;

        public GetWeather(ILogger<GetWeather> logger)
        {
            _logger = logger;
        }

        [HttpGet("/api/weather")]
        [SwaggerOperation(Tags = new[] { SwaggerGroup.Weather })]
        public override async Task<ActionResult<IEnumerable<WeatherForecast>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            Response.AddMockPaginationHeaders();

            return Ok(weather);
        }
    }
}