namespace SharpService.ServiceClients
{
    public interface IServiceClientProviderFactory
    {
        IServiceClientProvider Get();

        IServiceClientProvider Get(string protocol);
    }
}
