
using System;
using System.ServiceModel;


namespace M9_WCF_Service
{
    [ServiceContract(Namespace="http://www.4dotnet.nl/WCF")]
    public interface IMath
    {
        [OperationContract]
        int Add(int a, int b);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginSubtract(int a, int b, AsyncCallback cb, object asyncState);
        int EndSubtract(IAsyncResult ar);
    }
}
