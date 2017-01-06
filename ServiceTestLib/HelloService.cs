using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using TheService.Extension.Service;

namespace ServiceTestLib
{
    public class HelloService : IHelloService
    {

        public string GetMessage(string name)
        {
            var msg = ServiceUtils.GetHeaderValue("test");
            return "Hello " + name + msg;
        }

        public Person GetMessage1(Person person, Student student)
        {
            person.PerName = "TestName";
            return person;
        }
    }

    public class Person
    {
        public string PerName { set; get; }
    }

    public class Student
    {
        public string StuName { set; get; }
    }

}
