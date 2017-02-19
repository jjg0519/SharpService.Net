using System.Collections.Generic;
using System.ServiceModel;

namespace Sample.Service
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        void TestVoid();

        [OperationContract]
        string TestArgVoid();

        [OperationContract]
        string TestString(string arg);

        [OperationContract]
        int TestInt(int i);

        [OperationContract]
        bool TestBoolean(bool b);

        [OperationContract]
        long TestLong(long l);

        [OperationContract]
        float TestFloat(float f);

        [OperationContract]
        double TestDouble(double d);

        [OperationContract]
        List<object> TestList(List<object> list);

        [OperationContract]
        Dictionary<string, object> TestMap(Dictionary<string, object> map);
    }
}
