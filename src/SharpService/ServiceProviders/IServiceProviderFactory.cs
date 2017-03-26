using SharpService.Configuration;

namespace SharpService.ServiceProviders
{
    public interface IServiceProviderFactory
    {
        IServiceProvider Get();

        IServiceProvider Get(string protocol);
    }
}
