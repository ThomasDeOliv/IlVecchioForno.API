using Serilog;
using Serilog.Settings.Configuration;

namespace IlVecchioForno.API.Logger;

public static class SerilogConfiguration
{
    private const string ConfigurationSectionName = "Serilog";

    public static void ConfigureSerilog(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(
                configuration,
                new ConfigurationReaderOptions
                {
                    SectionName = ConfigurationSectionName
                }
            )
            .CreateLogger();
    }
}