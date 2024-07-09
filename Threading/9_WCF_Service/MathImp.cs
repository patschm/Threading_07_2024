using System;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;

namespace M9_WCF_Service
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class MathImp : IMath
    {
        private int timesCalled = 0;

        public int Add(int a, int b)
        {
            Console.WriteLine("Add is called {0} times", ++timesCalled);
            return a + b;
        }

        public IAsyncResult BeginSubtract(int a, int b, AsyncCallback cb, object asyncState)
        {
            Func<int, int, int> del = (_a, _b) => { System.Threading.Thread.Sleep(4000); return _a - _b; };
            return del.BeginInvoke(a, b, cb, asyncState);
        }

        public int EndSubtract(IAsyncResult ar)
        {
            AsyncResult ares = ar as AsyncResult;
            Func<int, int, int> md = ares.AsyncDelegate as Func<int, int, int>;
            return md.EndInvoke(ar);
        }
    }
}
