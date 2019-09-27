using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp2;

namespace ClassLibrary
{
    public struct Value
    {
        public void Foo() { }
    }

    public class Base
    {
        public virtual void Foo() { }

        public static void Static() { }
    }

    public sealed class Derived : Base
    {
        public sealed override void Foo()
        {
            // [1] call instance void Base::Foo()
            base.Foo();
        }

        public void Bar() { }

        public void FooBar()
        {
            // [2] callvirt instance void Base::Foo()
            this.Foo();

            // [3] call instance void Derived::Bar()
            this.Bar();
        }
    }

    class Demo
    {
        static void CallMethods()
        {
            var value = new Value();

            // [4] call instance bool Value::IsPositive()
            value.Foo();

            // [5] box Value
            //     call instance class System.Type System.Object::GetType()
            value.GetType();

            // [6] call void Base::Static()
            Base.Static();

            var derived = new Derived();

            // [7] callvirt instance void Derived::Bar()
            derived.Bar();
            // [8] call instance void Derived::Bar()
            new Derived().Bar();

            // [9] callvirt instance void Base::Foo()
            derived.Foo();
            // [10] callvirt instance void Base::Foo()
            new Derived().Foo();
        }
    }
}
