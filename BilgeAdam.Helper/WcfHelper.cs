using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace BilgeAdam.Helper
{
    public class WcfHelper
    {
        //servisin kendi kendine çalışmasını sağlayacak
        public static ServiceHost Run<T1,T2>(string ip, int port, string serviceName)
        {
            Uri uri = new Uri("http://"+ip+":"+port+"/"+serviceName);
            ServiceHost serviceHost = new ServiceHost(typeof(T1), uri);

            
            serviceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults=true});

            //HttpCon açıyoruz
            ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior { HttpGetEnabled=true};

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                ReceiveTimeout = new TimeSpan(int.MaxValue),
                SendTimeout = new TimeSpan(int.MaxValue),
                TransferMode = TransferMode.Streamed
            };

            serviceHost.AddServiceEndpoint(typeof(T2), basicHttpBinding, uri);
            serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);

            serviceHost.Open();
            Console.WriteLine("WCF is live at:"+ uri);

            return serviceHost;
        }

        public class Wcf<T1>
        {
            private static T1 _channelS = default(T1);

            public static T1 Channel(string wcfCommonEndPoint)
            {
                if (_channelS != null) return _channelS;

                BasicHttpBinding basicHttpBinding = new BasicHttpBinding
                {
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    MaxBufferPoolSize = int.MaxValue,
                    ReceiveTimeout = new TimeSpan(int.MaxValue),
                    SendTimeout = new TimeSpan(int.MaxValue),
                    TransferMode = TransferMode.Streamed
                };


                EndpointAddress endpoint = new EndpointAddress(wcfCommonEndPoint);
                ChannelFactory<T1> factory = new ChannelFactory<T1>(basicHttpBinding, endpoint);
                T1 channel = factory.CreateChannel();
                _channelS = channel;

                return channel;

            }
        }
    }
}
