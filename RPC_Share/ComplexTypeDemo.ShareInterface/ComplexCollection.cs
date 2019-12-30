using System;
using System.Collections.Generic;


namespace ComplexTypeDemo.ShareInterface
{
    public class ComplexCollection : List<MyType>
    {
    }

    public class MyType
    {
        public Guid ID { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public override string ToString()
        {
            return $"{{ ID: {ID}, Message: {Message}, StatusCode: {StatusCode}}}";
        }
    }
}
