namespace IlVecchioForno.API.Cors;

public static class DefaultCorsConfiguration
{
    public static IServiceCollection AddDefaultCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithMethods("GET", "POST", "PATCH")
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        return services;
    }
}