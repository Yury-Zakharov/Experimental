using IntelliFlo.AppStartup;

namespace Microservice.Experimental.Platform;

/// <inheritdoc />
public sealed class Startup :MicroserviceStartup
{
    /// <inheritdoc />
    public Startup(IConfiguration configuration) : base(configuration)
    {
    }
}