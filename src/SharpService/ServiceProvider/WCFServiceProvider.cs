using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ProtoBuf.ServiceModel;
using SharpService.Utilities;
using SharpService.Configuration;
using System.ServiceModel.Description;
using SharpService.Behavior;

namespace SharpService.ServiceProvider
{
    public class WCFServiceProvider : IServiceProvider
    {
        private const string serviceConfig = "serviceGroup/serviceConfig";
        private const string classConfig = "serviceGroup/classConfig";
        private static List<ServiceHost> hosts = new List<ServiceHost>();
        private readonly List<Configuration.ServiceConfiguration> serviceConfigurations = ConfigurationManager.GetSection(serviceConfig) as List<Configuration.ServiceConfiguration>;
        private readonly List<ClassConfiguration> classConfigurations = ConfigurationManager.GetSection(classConfig) as List<ClassConfiguration>;   
        private static AutoResetEvent providerEvent = new AutoResetEvent(false);

        public IServiceProvider Provider()
        {
            int serviceCount = 0;
            foreach (var serviceConfiguration in serviceConfigurations)
            {
                if (serviceConfiguration.Enable == false)
                    continue;
                var classsConfiguration = classConfigurations.FirstOrDefault(x => x.Id == serviceConfiguration.Ref);
                if (classsConfiguration == null)
                {
                    throw new ArgumentNullException("classsConfiguration can not find !");
                }
                Type serviceType = null;
                if (!string.IsNullOrEmpty(classsConfiguration.Assembly))
                {
                    var assembly = Assembly.LoadFrom(FileUtil.GetAbsolutePath(string.Format("{0}.dll", classsConfiguration.Assembly)));
                    serviceType = assembly.GetType(classsConfiguration.Type);
                }
                else
                {
                    throw new ArgumentNullException("serviceType assembly can not find !");
                }
                if (serviceType == null)
                {
                    throw new ArgumentNullException(string.Format("serviceType can not find  type {0} assembly {1}  !", classsConfiguration.Type, classsConfiguration.Assembly));
                }
                var host = new ServiceHost(serviceType);
                Type implementedContract = null;
                if (!string.IsNullOrEmpty(serviceConfiguration.Assembly))
                {
                    var assembly = Assembly.LoadFrom(FileUtil.GetAbsolutePath(string.Format("{0}.dll", serviceConfiguration.Assembly)));
                    implementedContract = assembly.GetType(serviceConfiguration.Interface);
                }
                else
                {
                    throw new ArgumentNullException("implementedContract assembly can not find !");
                }
                if (implementedContract == null)
                {
                    throw new ArgumentNullException(string.Format("implementedContract can not find type {0} assembly {1} !", serviceConfiguration.Interface, serviceConfiguration.Assembly));
                }

                var endpoint = host.AddServiceEndpoint(
                      implementedContract,
                      ConfigurationHelper.CreateBinding(serviceConfiguration.Binding, (SecurityMode)serviceConfiguration.Security),
                      new Uri(serviceConfiguration.Address));
                endpoint.Behaviors.Add(new ProtoEndpointBehavior());
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    var behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = serviceConfiguration.Binding.Contains("http") ? true : false;
                    behavior.HttpGetUrl = serviceConfiguration.Binding.Contains("http") ?
                                                        new Uri($"{host.Description.Endpoints[0].Address.Uri.ToString()}/mex") :
                                                        null;
                    host.Description.Behaviors.Add(behavior);
                }
                if (host.Description.Behaviors.Find<ErrorServiceBehavior>() == null)
                {
                    var behavior = new ErrorServiceBehavior();
                    host.Description.Behaviors.Add(behavior);
                }
                hosts.Add(host);
                host.Opened += (sender, o) =>
                {
                    serviceCount++;
                    if (serviceConfigurations.Count(x => x.Enable) == serviceCount)
                    {
                        providerEvent.Set();
                    }
                };
                if (host.State != CommunicationState.Opening)
                    host.Open();
            }
            providerEvent.WaitOne();
            return this;
        }

        public IServiceProvider Close()
        {
            foreach (var host in hosts)
            {
                if (host.State == CommunicationState.Opened)
                    host.Close();
            }
            hosts.Clear();
            return this;
        }
    }
}
