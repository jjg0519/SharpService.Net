using SharpService.Configuration;
using System.Threading.Tasks;

namespace SharpService.ServiceProvider
{
    public interface IServiceProviderFactory
    {
        IServiceProvider Get();

        IServiceProvider Get(ProtocolConfiguration protocolConfig);
    }
}
