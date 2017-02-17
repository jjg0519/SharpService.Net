namespace SharpService.Factory
{
    public interface IServiceProvider
    {
        IServiceProvider ProviderService();

        IServiceProvider RegistryService();

        IServiceProvider CloseService();

        IServiceProvider CancelService();
    }
}
