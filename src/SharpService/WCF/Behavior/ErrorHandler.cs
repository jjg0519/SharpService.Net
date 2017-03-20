using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace SharpService.WCF.Behavior
{
    public class ErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var ex = new FaultException<ExceptionDetail>(new ExceptionDetail(error), error.Message);
            var mf = ex.CreateMessageFault();
            fault = Message.CreateMessage(version, mf, ex.Action);
        }
    }
}
