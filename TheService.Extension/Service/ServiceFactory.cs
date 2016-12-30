﻿using TheService.Extension.ConfigFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using TheService.Common;

namespace TheService.Extension
{
    public class ServiceFactory
    {
        private static string serviceConfig = "serviceGroup/serviceConfig";
        private static string referenceConfig = "serviceGroup/referenceConfig";

        private static List<ServiceHost> hosts = new List<ServiceHost>();

        private static List<ServiceElement> services = ConfigurationManager.GetSection(serviceConfig) as List<ServiceElement>;
        private static  List<ReferenceElement> references = ConfigurationManager.GetSection(referenceConfig) as List<ReferenceElement>;

        public static bool Start()
        {
            int serviceCount = 0;
        

            #region InitService
            if (services != null)
            {
                foreach (ServiceElement serviceElement in services)
                {
                    if (serviceElement.Enable == false)
                        continue;
                    ReferenceElement referenceElement = references.FirstOrDefault(x => x.Id == serviceElement.Ref);
                    if (referenceElement == null)
                    {
                        throw new Exception("referenceElement can not find !");
                    }
                    Type serviceType = null;
                    if (!string.IsNullOrEmpty(referenceElement.Assembly))
                    {
                        Assembly assembly = Assembly.LoadFrom(FileHelper.GetAbsolutePath(string.Format("{0}.dll", referenceElement.Assembly)));
                        serviceType = assembly.GetType(referenceElement.Type);
                    }
                    else
                    {
                        serviceType = Type.GetType(referenceElement.Type);
                    }
                    if (serviceType == null)
                    {
                        throw new Exception(string.Format("serviceType can not find  type {0} assembly {1}  !", referenceElement.Type, referenceElement.Assembly));
                    }
                    ServiceHost host = new ServiceHost(serviceType);
                    Type implementedContract = null;
                    if (!string.IsNullOrEmpty(serviceElement.Assembly))
                    {
                        Assembly assembly = Assembly.LoadFrom(FileHelper.GetAbsolutePath(string.Format("{0}.dll", serviceElement.Assembly)));
                        implementedContract = assembly.GetType(serviceElement.Interface);
                    }
                    else
                    {
                        implementedContract = Type.GetType(serviceElement.Interface);
                    }
                    if (implementedContract == null)
                    {
                        throw new Exception(string.Format("implementedContract can not find type {0} assembly {1} !", serviceElement.Interface, serviceElement.Assembly));
                    }

                    host.AddServiceEndpoint(implementedContract, ConfigFactory.ConfigHelper.CreateBinding(serviceElement.Binding, (SecurityMode)serviceElement.Security), new Uri(serviceElement.Address));
                    //if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                    //{
                    //    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    //    behavior.HttpGetEnabled = true;
                    //    host.Description.Behaviors.Add(behavior);
                    //}
                    hosts.Add(host);
                    host.Opened += (sender, o) =>
                    {
                        ServiceHost _host = sender as ServiceHost;
                        string _serviceType = _host.Description.ConfigurationName;
                        string _interface = _host.Description.Endpoints[0].Contract.ContractType.FullName;
                        string _endpoint = string.Join(";", _host.Description.Endpoints.ToList().Select(s => s.Address));
                        Console.WriteLine(string.Format("service already started serviceType {0}  interface {1}  endpoint {2}  ", _serviceType, _interface, _endpoint));
                        serviceCount++;
                    };
                    if (host.State != CommunicationState.Opening)
                        host.Open();
                }
            }
            #endregion
            while (true)
            {
                if (serviceCount == services.Where(x => x.Enable).Count())
                    break;
            }
            return true;
        }

        public static void Stop()
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
