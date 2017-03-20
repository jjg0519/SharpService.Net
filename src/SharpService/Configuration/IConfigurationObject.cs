using System.Collections.Generic;

namespace SharpService.Configuration
{
    public interface IConfigurationObject
    {
        List<RefererConfiguration> refererConfigurations { get; }

        List<ServiceConfiguration> serviceConfigurations { get; }

        List<ClassConfiguration> classConfigurations { get; }

        RegistryConfiguration registryConfiguration { get; }

        ProtocolConfiguration protocolConfiguration { get; }
    }
}
