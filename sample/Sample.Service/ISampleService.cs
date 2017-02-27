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
        List<string> TestList(List<string> list);

        [OperationContract]
        Dictionary<string, string> TestMap(Dictionary<string, string> map);
    }
}
