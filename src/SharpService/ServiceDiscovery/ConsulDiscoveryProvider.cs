using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharpService.Configuration;
using Consul;
using SharpService.Components;
using System.Linq;
using SemVer;
using SharpService.Utilities;
using System.Net;

namespace SharpService.ServiceDiscovery
{
    public class ConsulDiscoveryProvider : IServiceDiscoveryProvider
    {     
        private readonly ConsulClient _consul;

        private IConfigurationObject configuration { get; set; }

        public ConsulDiscoveryProvider()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();

            _consul = new ConsulClient(config =>
            {
                config.Address = new Uri(configuration.registryConfiguration.Address);

            });
        }

        public async Task<List<RegistryInformation>> FindAllServicesAsync()
        {
            var queryResult = await _consul.Agent.Services();
            var instances = queryResult.Response.Select(serviceEntry => new RegistryInformation
            {
                Name = serviceEntry.Value.Service,
                Id = serviceEntry.Value.ID,
                Address = serviceEntry.Value.Address,
                Port = serviceEntry.Value.Port,
                Version = ConsulHelper.GetVersionFromStrings(serviceEntry.Value.Tags),
                Tags = serviceEntry.Value.Tags
            });

            return instances.ToList();
        }

        public async Task<List<RegistryInformation>> FindServicesAsync(string name)
        {
            var queryResult = await _consul.Health.Service(name, tag: "", passingOnly: true);
            var instances = queryResult.Response.Select(serviceEntry => new RegistryInformation
            {
                Name = serviceEntry.Service.Service,
                Address = serviceEntry.Service.Address,
                Port = serviceEntry.Service.Port,
                Version = ConsulHelper.GetVersionFromStrings(serviceEntry.Service.Tags),
                Tags = serviceEntry.Service.Tags ?? Enumerable.Empty<string>(),
                Id = serviceEntry.Service.ID
            });

            return instances.ToList();
        }

        public async Task<List<RegistryInformation>> FindServicesWithVersionAsync(string name, string version)
        {
            var instances = await FindServicesAsync(name);
            var range = new Range(version);

            return instances.Where(x => range.IsSatisfied(x.Version)).ToList();
        }

        public async Task RegisterServiceAsync()
        {
            foreach (var serviceConfiguration in configuration.serviceConfigurations)
            {
                await RegisterServiceAsync(serviceConfiguration, configuration.protocolConfiguration, null, null);
            }
        }

        public async Task<RegistryInformation> RegisterServiceAsync(ServiceConfiguration serviceConfiguration, ProtocolConfiguration protocolConfiguration, Uri healthCheckUri = null, IEnumerable<string> tags = null)
        {
            var serviceId = ConsulHelper.GetServiceId(serviceConfiguration.Interface, serviceConfiguration.Port.ToString());
            var serviceName = ConsulHelper.GetServiceName(protocolConfiguration.Protocol, serviceConfiguration.Interface);

            string versionLabel = $"{ConsulHelper.VERSION_PREFIX}{serviceConfiguration.Version}";
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();
            tagList.Add(protocolConfiguration.Protocol);
            tagList.Add(protocolConfiguration.Transmit);
            tagList.Add(versionLabel);

            string check = healthCheckUri?.ToString() ?? $"";
            var agentServiceCheck = protocolConfiguration.Transmit == "tcp" ?
                                                    new AgentServiceCheck { TCP = check, Interval = TimeSpan.FromSeconds(2) }
                                                    : new AgentServiceCheck { HTTP = check, Interval = TimeSpan.FromSeconds(2) };

            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = serviceName,
                Tags = tagList.ToArray(),
                Address = await DnsUtil.GetIpAddressAsync(),
                Port = serviceConfiguration.Port,
                Check = new AgentServiceCheck { HTTP = check, Interval = TimeSpan.FromSeconds(2) }
            };

            await _consul.Agent.ServiceRegister(registration);

            return new RegistryInformation
            {
                Name = registration.Name,
                Id = registration.ID,
                Address = registration.Address,
                Port = registration.Port,
                Version = serviceConfiguration.Version,
                Tags = tagList
            };

        }

        public async Task DeregisterServiceAsync()
        {
            foreach (var serviceConfiguration in configuration.serviceConfigurations)
            {
                var serviceId = ConsulHelper.GetServiceId(serviceConfiguration.Interface, serviceConfiguration.Port.ToString());
                await DeregisterServiceAsync(serviceId);
            }
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var writeResult = await _consul.Agent.ServiceDeregister(serviceId);
            bool isSuccess = writeResult.StatusCode == HttpStatusCode.OK;
            string success = isSuccess ? "succeeded" : "failed";

            return isSuccess;
        }
    }
}
