using System;
using System.ServiceModel;
using System.Threading;

namespace Host
{
    class Program
    {
        private static string WebProxyServiceUrl = "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/";
        private static string RoutingUrl = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/";

        static void Main(string[] args)
        {
            Uri webPorxyServiceBaseAdr = new Uri(WebProxyServiceUrl);
            Uri routingWebServiceBaseAdr = new Uri(RoutingUrl);

            Thread tProxy = new Thread(launchWebProxy);
            Thread tRouting = new Thread(launchRouting);

            tProxy.Start(webPorxyServiceBaseAdr);
            tRouting.Start(routingWebServiceBaseAdr);

            tProxy.Join();
            tRouting.Join();
        }

        private static void launchWebProxy(object webPorxyServiceBaseAdr)
        {
            using (ServiceHost selfHost = new ServiceHost(typeof(WebProxyService.Service1)))
            {
                try
                {
                    selfHost.Open();
                    Console.WriteLine("The WebProxyService is ready. {0}", selfHost.BaseAddresses[0]);

                    Console.WriteLine("Press <Enter> to terminate the service.");
                    Console.WriteLine();
                    Console.ReadLine();
                    Console.WriteLine("The WebProxyService is closed");
                    selfHost.Close();
                }
                catch (CommunicationException ce)
                {
                    Console.WriteLine("An exception occurred: {0}", ce.Message);
                    selfHost.Abort();
                }
            };
        }

        private static void launchRouting(object routingWebServiceBaseAdr)
        {
            using (ServiceHost selfHost = new ServiceHost(typeof(Routing.Service1)))
            {
                try
                {
                    selfHost.AddServiceEndpoint(typeof(Routing.IService1), new BasicHttpBinding(), "Routing");
                    selfHost.Open();
                    Console.WriteLine("The Routingservice is ready. {0}", selfHost.BaseAddresses[0]);

                    Console.WriteLine("Press <Enter> to terminate the service.");
                    Console.WriteLine();
                    Console.ReadLine();
                    Console.WriteLine("The Routingservice is closed");
                    selfHost.Close();
                }
                catch (CommunicationException ce)
                {
                    Console.WriteLine("An exception occurred: {0}", ce.Message);
                    selfHost.Abort();
                }
            };
        }
    }
}
