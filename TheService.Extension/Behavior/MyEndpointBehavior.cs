using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using TheService.Extension.Formatter;

namespace TheService.Extension.Behavior
{
    public class MyEndpointBehavior : IEndpointBehavior, IOperationBehavior
    {
        private Dictionary<string, string> messages { set; get; }

        public MyEndpointBehavior(Dictionary<string, string> messages)
        {
            this.messages = messages;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AttachMessageBehavior(messages));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void Validate(OperationDescription operationDescription)
        {

        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Formatter = new NewtonsoftJsonDispatchFormatter(operationDescription, true);
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Formatter = new NewtonsoftJsonClientFormatter(operationDescription);
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {

        }
    }
}
