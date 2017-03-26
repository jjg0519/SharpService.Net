using System.Collections.Generic;

namespace SharpService.ServiceProviders
{
    public interface IServiceProvider
    {
        void Provider(List<Configuration.ServiceConfiguration> serviceConfigurations);

        void Close();

    }
}
