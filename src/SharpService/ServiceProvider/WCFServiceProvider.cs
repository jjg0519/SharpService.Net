using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using ProtoBuf.ServiceModel;
using SharpService.Utilities;
using SharpService.Configuration;
using System.ServiceModel.Description;
using SharpService.WCF.Behavior;
using SharpService.Components;
using SharpService.WCF;

namespace SharpService.ServiceProvider
{
    public class WCFServiceProvider : IServiceProvider
    {      
        private IConfigurationObject configuration{ get; set; }

        private  List<ServiceHost> hosts = new List<ServiceHost>();

        public WCFServiceProvider()
        {
            configuration = ObjectContainer.Resolve<IConfigurationObject>();
        }

        private  AutoResetEvent providerEvent = new AutoResetEvent(false);

        public void Provider()
        {
            int serviceCount = 0;
            foreach (var serviceConfiguration in configuration.serviceConfigurations)
            {
                var classsConfiguration = configuration.classConfigurations.FirstOrDefault(x => x.Id == serviceConfiguration.Ref);
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
                var address = WCFHelper.CreateAddress(
                    configuration.protocolConfiguration.Transmit,
                    serviceConfiguration.Port.ToString(),
                    implementedContract.Name.TrimStart('I').ToLower());
                var endpoint = host.AddServiceEndpoint(
                      implementedContract,
                      WCFHelper.CreateBinding(configuration.protocolConfiguration.Transmit),
                      new Uri(address));
                endpoint.Behaviors.Add(new ProtoEndpointBehavior());
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    var behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = configuration.protocolConfiguration.Transmit.Contains("http") ? true : false;
                    behavior.HttpGetUrl = configuration.protocolConfiguration.Transmit.Contains("http") ?
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
                    if (configuration.serviceConfigurations.Count() == serviceCount)
                    {
                        providerEvent.Set();
                    }
                };
                if (host.State != CommunicationState.Opening)
                    host.Open();
            }
            providerEvent.WaitOne();
        }

        public void Close()
        {
            foreach (var host in hosts)
            {
                if (host.State == CommunicationState.Opened)
                    host.Close();
            }
            hosts.Clear();
        }
    }
}
