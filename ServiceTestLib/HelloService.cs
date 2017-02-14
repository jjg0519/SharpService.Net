using ProtoBuf;
using TheService.Extension.Service;

namespace ServiceTestLib
{
    public class HelloService : IHelloService
    {

        public string GetMessage(string name)
        {
            return "Hello " + name;
        }

        public Person GetMessage1(Person person)
        {
            person.PerName = "TestName";
            return person;
        }
    }

    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public string PerName { set; get; }
    }
}
