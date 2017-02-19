using System.ServiceModel;

namespace ServiceTestLib
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        void Error();

        [OperationContract]
        void OK();

        [OperationContract]
        void TimeOut();

        [OperationContract]
        string SendMessage(string message);

        [OperationContract]
        Data SendData(Data person);
    }
}
