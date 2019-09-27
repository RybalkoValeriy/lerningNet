using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClassLibrary1
{
    // class for access through appdomain 
    public sealed class MarshalByRefType : MarshalByRefObject
    {
        public MarshalByRefType() =>
            Console.WriteLine($"{this.GetType()} ctor running " +
                              $"in {Thread.GetDomain().FriendlyName}");

        public void SomeMethod() =>
            Console.WriteLine($"executing in {Thread.GetDomain().FriendlyName}");

        public MarshalByValType MethodWithReturn()
        {
            this.SomeMethod();
            MarshalByValType t = new MarshalByValType();
            return t;
        }

        public NonMarshalingType MethodArgsAndReturn(string callingDomainName)
        {
            Console.WriteLine($"Calling from {callingDomainName} " +
                              $"to {Thread.GetDomain().FriendlyName}");
            NonMarshalingType t = new NonMarshalingType();
            return t;
        }
    }

    // instance access to domain
    [Serializable]
    public class MarshalByValType
    {
        private readonly DateTime _creationTime = DateTime.Now;

        public MarshalByValType()
            => Console.WriteLine(
                $"{this.GetType()} ctor running " +
                $"in {Thread.GetDomain().FriendlyName}, " +
                $"created on {_creationTime}");

        public override string ToString()
            => _creationTime.ToLongDateString();
    }

    // instance no access to domain
    public class NonMarshalingType
    {
        public NonMarshalingType()
            => Console.WriteLine($"Executing in {Thread.GetDomain().FriendlyName}");
    }
}
