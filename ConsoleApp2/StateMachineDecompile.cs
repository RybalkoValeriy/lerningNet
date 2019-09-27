using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Pr
    {
        public static async Task<string> MyMethodAsync(int arg)
        {
            int local = arg;
            try
            {
                Type1 result1 = await Method1Async();
                for (int i = 0; i < 3; i++)
                {
                    Type2 result2 = await Method2Async();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch");
            }
            finally
            {
                Console.WriteLine("finally");
            }

            return "done";
        }

        public static async Task<Type1> Method1Async()
        {
            Task<Type1> t = new Task<Type1>(() => new Type1());
            t.Start();
            t.Wait(2000);
            return await t;
        }

        public static async Task<Type2> Method2Async()
        {
            Task<Type2> t = new Task<Type2>(() => new Type2());
            t.Start();
            t.Wait(1000);
            return await t;
        }
    }
    public class Type1 { }
    public class Type2 { }

    public class StateMachineDecompile
    {
        [DebuggerStepThrough, AsyncStateMachine(typeof(StateMachine))]
        internal static Task<string> MyMethodAsync(int arg)
        {
            StateMachine stateMachine = new StateMachine
            {
                builder = AsyncTaskMethodBuilder<string>.Create(),
                state = 1,
                argument = arg
            };

            stateMachine.builder.Start(ref stateMachine);
            return stateMachine.builder.Task;
        }
    }

    // structure statue machine
    [CompilerGenerated, StructLayout(LayoutKind.Auto)]
    internal struct StateMachine : IAsyncStateMachine
    {
        // for task
        public AsyncTaskMethodBuilder<string> builder;
        public int state;

        // argument and local variable
        public int argument;
        public int local;
        public int x;
        public Type1 resultType1;
        public Type2 resultType2;

        // file awaiter
        private TaskAwaiter<Type1> _awaiterType1;
        private TaskAwaiter<Type2> _awaiterType2;

        // stateMachine
        void IAsyncStateMachine.MoveNext()
        {
            string result = null; // result Task;
            try
            {
                bool executeFinally = true;

                if (this.state == 1)
                    this.local = this.argument; // if statemachine run first inilials state

                try
                {
                    TaskAwaiter<Type1> awaiterType1;
                    TaskAwaiter<Type2> awaiterType2;

                    switch (this.state)
                    {
                        // first state
                        case 1:
                            // get object awaiter
                            awaiterType1 = Pr.Method1Async().GetAwaiter();
                            if (awaiterType1.IsCompleted == false)
                            {
                                this.state = 0;
                                this._awaiterType1 = awaiterType1;

                                // after completed run MoveNext
                                this.builder.AwaitUnsafeOnCompleted(ref awaiterType1, ref this);
                                executeFinally = false;
                                return;
                            }
                            else // posobility method compleate synchro
                                break;

                        // complete asynchrony method1
                        case 0:
                            // recovery last waiter object;
                            awaiterType1 = this._awaiterType1;
                            break;

                        // complete async method2
                        case 2:
                            awaiterType2 = this._awaiterType2;
                            goto ForLoopEpilog;
                    }

                    this.resultType1 = awaiterType1.GetResult();

                ForLoopPrologue:
                    this.x = 0;
                    goto ForLoopBody;

                ForLoopEpilog:
                    this.resultType2 = awaiterType2.GetResult();
                    this.x++;

                ForLoopBody:
                    if (this.x < 3)
                    {
                        awaiterType2 = Pr.Method2Async().GetAwaiter();
                        if (awaiterType2.IsCompleted == false)
                        {
                            this.state = 1;
                            this._awaiterType2 = awaiterType2;

                            this.builder.AwaitUnsafeOnCompleted(ref awaiterType2, ref this);
                            executeFinally = false;
                            return;
                        }

                        goto ForLoopEpilog;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Catch");
                }
                finally
                {
                    if (executeFinally)
                        Console.WriteLine("finally");
                }

                result = "Done";
            }
            catch (Exception exception)
            {
                this.builder.SetException(exception);
                return;
            }
            this.builder.SetResult(result);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }
}
