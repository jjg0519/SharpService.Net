using System;
using System.Collections.Generic;
using Sample.Service;

namespace Sample.Business
{
    public class SampleBusiness : ISampleService
    {
        public string TestArgVoid()
        {
            return "TestArgVoid";
        }

        public bool TestBoolean(bool b)
        {
            return true;
        }

        public double TestDouble(double d)
        {
            return d;
        }

        public float TestFloat(float f)
        {
            return f;
        }

        public int TestInt(int i)
        {
            return i;
        }

        public List<object> TestList(List<object> list)
        {
            return list;
        }

        public long TestLong(long l)
        {
            return l;
        }

        public Dictionary<string, object> TestMap(Dictionary<string, object> map)
        {
            return map;
        }

        public string TestString(string arg)
        {
            return arg;
        }

        public void TestVoid()
        {           
        }
    }
}
