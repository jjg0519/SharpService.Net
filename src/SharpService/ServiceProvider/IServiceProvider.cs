namespace SharpService.ServiceProvider
{
    public interface IServiceProvider
    {
        IServiceProvider Provider();

        IServiceProvider Close();

    }
}
