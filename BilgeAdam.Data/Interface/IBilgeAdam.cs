using System;
using System.ServiceModel;

namespace BilgeAdam.Data.Interface
{
    [ServiceContract]
    public interface IBilgeAdam
    {
        [OperationContract]
        void Test1();

        [OperationContract]
        void MyName(string name, DateTime myDate);

        [OperationContract]
        DateTime ServerDate();
    }
}
