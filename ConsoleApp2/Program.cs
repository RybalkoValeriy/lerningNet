using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public sealed class Program
    {
        public static void Main()
        {
            StateMachineAsyncAwait
                .SumAsync(10, 10)
                .ContinueWith((t) => Console.WriteLine(t.Result));

            //SumAsync(10, 10)
            //    .ContinueWith((t) => Console.WriteLine(t.Result));

            Console.ReadKey();
        }

        public static int Sum(int args1, int args2)
        {

            return (args1 + args2);
        }

        public static async Task<int> SumAsync(int args1, int args2)
        {
            Info.WhichThread("start", 10);
            var task = new Task<int>(() => Sum(args1, args2));
            task.Start();

            var r = await task;
            Info.WhichThread("end", 10);

            return r;
        }

    }

    public static class Info
    {
        public static void WhichThread(string mess, int count)
        {
            Console.WriteLine($"ThreadID {Thread.CurrentThread.ManagedThreadId} mess-{mess}, count-{count}");
        }
    }
}