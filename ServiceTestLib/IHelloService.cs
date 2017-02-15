using System.ServiceModel;

namespace ServiceTestLib
{

    [ServiceContract]
    public interface IHelloService
    {     
        [OperationContract]
        string GetMessage(string message);

        [OperationContract]
        Person GetPerson(Person person);
    }
}
