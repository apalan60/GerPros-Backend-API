﻿using GerPros_Backend_API.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace GerPros_Backend_API.Web.Endpoints.Demo;

[Obsolete("This endpoint is for demonstration purposes only.")]
public class WeatherForecasts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        // app.MapGroup(this)
        //     .RequireAuthorization()
        //     .MapGet(GetWeatherForecasts);
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(ISender sender)
    {
        return await sender.Send(new GetWeatherForecastsQuery());
    }
}
