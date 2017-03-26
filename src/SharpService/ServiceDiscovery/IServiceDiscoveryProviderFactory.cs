namespace SharpService.ServiceDiscovery
{
    public interface IServiceDiscoveryProviderFactory
    {
        IServiceDiscoveryProvider Get();

        IServiceDiscoveryProvider Get(string regProtocol);
    }
}
