using System;
using System.ServiceModel;
using System.Threading;
using WebProxyService;
using IService1 = Routing.IService1;

namespace Host
{
    internal class Program
    {
        private static readonly string WebProxyServiceUrl =
            "http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/";

        private static readonly string RoutingUrl = "http://localhost:8733/Design_Time_Addresses/Routing/Service1/";

        private static void Main(string[] args)
        {
            var webPorxyServiceBaseAdr = new Uri(WebProxyServiceUrl);
            var routingWebServiceBaseAdr = new Uri(RoutingUrl);

            var tProxy = new Thread(launchWebProxy);
            var tRouting = new Thread(launchRouting);

            tProxy.Start(webPorxyServiceBaseAdr);
            tRouting.Start(routingWebServiceBaseAdr);

            tProxy.Join();
            tRouting.Join();
        }

        private static void launchWebProxy(object webPorxyServiceBaseAdr)
        {
            using (var selfHost = new ServiceHost(typeof(Service1)))
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
            }

            ;
        }

        private static void launchRouting(object routingWebServiceBaseAdr)
        {
            using (var selfHost = new ServiceHost(typeof(Routing.Service1)))
            {
                try
                {
                    selfHost.AddServiceEndpoint(typeof(IService1), new BasicHttpBinding(), "Routing");
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
            }

            ;
        }
    }
}