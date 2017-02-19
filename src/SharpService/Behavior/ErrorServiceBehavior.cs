using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SharpService.Behavior
{
    public class ErrorServiceBehavior : IServiceBehavior
    {

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            var errorHanlder = new ErrorHandler();
            foreach (ChannelDispatcher chanDisp in serviceHostBase.ChannelDispatchers)
            {
                chanDisp.ErrorHandlers.Add(errorHanlder);
            }

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }

    public class ErrorHandlerElement : System.ServiceModel.Configuration.BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ErrorServiceBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(ErrorServiceBehavior);
            }
        }
    }
}
