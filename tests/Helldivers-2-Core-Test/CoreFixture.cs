using Helldivers.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Helldivers.Core.Test;

public sealed class CoreFixture
{
    public ServiceProvider Services { get; }

    public CoreFixture()
    {
        var services = new ServiceCollection();
        services.AddHelldivers();

        Services = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        });
    }
}
