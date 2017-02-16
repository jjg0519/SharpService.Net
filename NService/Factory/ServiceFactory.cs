﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using ProtoBuf.ServiceModel;
using NService.Utilities;
using NService.Configuration;
using System.ServiceModel.Description;
using NService.Behavior;

namespace NService.Factory
{
    public class ServiceFactory
    {
        private static string serviceConfig = "serviceGroup/serviceConfig";
        private static string classConfig = "serviceGroup/classConfig";
        private static List<ServiceHost> hosts = new List<ServiceHost>();
        private static List<ServiceElement> services = ConfigurationManager.GetSection(serviceConfig) as List<ServiceElement>;
        private static  List<ClassElement> classs = ConfigurationManager.GetSection(classConfig) as List<ClassElement>;
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);

        public static bool Builder()
        {
            int serviceCount = 0;
            if (services != null)
            {
                foreach (ServiceElement serviceElement in services)
                {
                    if (serviceElement.Enable == false)
                        continue;
                    ClassElement referenceElement = classs.FirstOrDefault(x => x.Id == serviceElement.Ref);
                    if (referenceElement == null)
                    {
                        throw new ArgumentNullException("referenceElement can not find !");
                    }
                    Type serviceType = null;
                    if (!string.IsNullOrEmpty(referenceElement.Assembly))
                    {
                        Assembly assembly = Assembly.LoadFrom(FileUtil.GetAbsolutePath(string.Format("{0}.dll", referenceElement.Assembly)));
                        serviceType = assembly.GetType(referenceElement.Type);
                    }
                    else
                    {
                        serviceType = Type.GetType(referenceElement.Type);
                    }
                    if (serviceType == null)
                    {
                        throw new ArgumentNullException(string.Format("serviceType can not find  type {0} assembly {1}  !", referenceElement.Type, referenceElement.Assembly));
                    }
                    ServiceHost host = new ServiceHost(serviceType);
                    Type implementedContract = null;
                    if (!string.IsNullOrEmpty(serviceElement.Assembly))
                    {
                        Assembly assembly = Assembly.LoadFrom(FileUtil.GetAbsolutePath(string.Format("{0}.dll", serviceElement.Assembly)));
                        implementedContract = assembly.GetType(serviceElement.Interface);
                    }
                    else
                    {
                        implementedContract = Type.GetType(serviceElement.Interface);
                    }
                    if (implementedContract == null)
                    {
                        throw new ArgumentNullException(string.Format("implementedContract can not find type {0} assembly {1} !", serviceElement.Interface, serviceElement.Assembly));
                    }

                    var endpoint = host.AddServiceEndpoint(
                          implementedContract,
                          ConfigHelper.CreateBinding(serviceElement.Binding,  (SecurityMode)serviceElement.Security),
                          new Uri(serviceElement.Address));
                    endpoint.Behaviors.Add(new ProtoEndpointBehavior());
                    if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                    {
                        ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                        behavior.HttpGetEnabled = serviceElement.Binding.Contains("http") ? true : false;
                        behavior.HttpGetUrl = serviceElement.Binding.Contains("http") ? new Uri(host.Description.Endpoints[0].Address.Uri.ToString() + "/mex") : null;
                        host.Description.Behaviors.Add(behavior);
                    }
                    if (host.Description.Behaviors.Find<ErrorServiceBehavior>() == null)
                    {
                        ErrorServiceBehavior behavior = new ErrorServiceBehavior();
                        host.Description.Behaviors.Add(behavior);
                    }
                    hosts.Add(host);
                    host.Opened += (sender, o) =>
                    {
                        serviceCount++;
                        if (services.Count() == serviceCount)
                        {
                            resetEvent.Set();
                        }
                    };
                    if (host.State != CommunicationState.Opening)
                        host.Open();
                }
            }
            resetEvent.WaitOne();
            return true;
        }

        public static void Cancel()
        {
            foreach (ServiceHost host in hosts)
            {
                if (host.State == CommunicationState.Opened)
                    host.Close();
            }
            hosts.Clear();
        }
    }
}
