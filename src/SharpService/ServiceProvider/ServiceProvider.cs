using SharpService.Components;
using SharpService.ServiceDiscovery;
using System.Threading.Tasks;

namespace SharpService.ServiceProvider
{
    public  class ServiceProvider
    {
        private IServiceDiscoveryProvider serviceDiscoveryProvider { set; get; }
        private IServiceProvider serviceProvider { set; get; }

        public ServiceProvider()
        {
            serviceDiscoveryProvider = ObjectContainer.Resolve<IServiceDiscoveryProviderFactory>().Get();
            serviceProvider = ObjectContainer.Resolve<IServiceProviderFactory>().Get();
        }

        public async Task Provider()
        {
            serviceProvider.Provider();
            await serviceDiscoveryProvider.RegisterServiceAsync();
        }

        public async Task Close()
        {
            serviceProvider.Close();
            await serviceDiscoveryProvider.DeregisterServiceAsync();
        }
    }
}
