using ProtoBuf;

namespace ServiceTestLib
{
    public class HelloService : IHelloService
    {
        public string GetMessage(string message)
        {
            return message;
        }

        public Person GetPerson(Person person)
        {
            return person;
        }
    }

    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public string Name { set; get; }
    }
}
