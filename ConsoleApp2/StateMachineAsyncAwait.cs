using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class StateMachineAsyncAwait
    {
        public static int Sum(int args1, int args2)
        {
            return args1 + args2;
        }

        public static Task<int> SumAsync(int args1, int args2)
        {
            GenerateStateMachine stateMachine = new GenerateStateMachine();
            stateMachine.Args1 = args1;
            stateMachine.Args2 = args2;
            stateMachine.AsyncTaskMethodBuilder = AsyncTaskMethodBuilder<int>.Create();
            stateMachine.State = State.BeforeRun;
            stateMachine.AsyncTaskMethodBuilder.Start<GenerateStateMachine>(ref stateMachine);
            return stateMachine.AsyncTaskMethodBuilder.Task;
        }
    }

    enum State
    {
        BeforeRun = -1,
        Run = 0,
        End = -2
    }


    class GenerateStateMachine : IAsyncStateMachine
    {
        public State State;
        public AsyncTaskMethodBuilder<int> AsyncTaskMethodBuilder;
        public int Args1;
        public int Args2;
        private Task<int> _task;
        private int _result;
        private TaskAwaiter<int> _taskAwaiter;
        private readonly Action _action = () => Console.WriteLine("SommeText");


        private int _acc;

        void IAsyncStateMachine.MoveNext()
        {
            _acc++;
            int innerTmpResult;
            try
            {
                TaskAwaiter<int> awaiter;

                if (this.State != State.Run)
                {

                    Info.WhichThread("state not run", this._acc);

                    this._task = new Task<int>(() => StateMachineAsyncAwait.Sum(this.Args1, this.Args2));
                    this._task.Start();
                    awaiter = this._task.GetAwaiter();

                    if (awaiter.IsCompleted == false)
                    {
                        Info.WhichThread("awaiter is completed=true", this._acc);

                        this.State = State.Run;
                        this._taskAwaiter = awaiter;


                        GenerateStateMachine stateMachine = this;
                        this.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
                        this._taskAwaiter.OnCompleted(this._action);

                        return;
                    }
                }
                else
                {
                    Info.WhichThread("state run", this._acc);

                    awaiter = this._taskAwaiter;
                    this._taskAwaiter = new TaskAwaiter<int>();
                    this.State = State.BeforeRun;
                }

                this._result = awaiter.GetResult();
                innerTmpResult = this._result;
            }

            catch (Exception ex)
            {
                this.State = State.End;
                this.AsyncTaskMethodBuilder.SetException(ex);
                return;
            }

            Info.WhichThread("state end", this._acc);
            Console.WriteLine($"acc-{this._acc}");

            this.State = State.End;
            this.AsyncTaskMethodBuilder.SetResult(innerTmpResult);
        }

        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
