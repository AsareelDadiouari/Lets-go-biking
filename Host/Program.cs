using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading;
using WebProxyService;
using IService1 = Routing.IService1;

namespace Host
{
    internal class Program
    {
        private static readonly string pathToLight = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).Parent.FullName + "\\LightClient\\ClientApp";

        private static void Main(string[] args)
        {
            var tProxy = new Thread(LaunchWebProxy);
            var tRouting = new Thread(LaunchRouting);
            var tNpmInstall = new Thread(LaunchNpmInstall);
            var tNpmStart = new Thread(LaunchNpmStart);

            tProxy.Start();
            tRouting.Start();

            if (Directory.Exists(pathToLight + "\\node_modules") == false)
            {
                tNpmInstall.Start();
                tNpmInstall.Join();
            }
               
            if (tNpmInstall.IsAlive == false)
            {
                tNpmStart.Start();
                tProxy.Join();
                tRouting.Join();
                tNpmStart.Join();
            }

            if (tRouting.IsAlive == false || tProxy.IsAlive == false)
            {
                tNpmStart.Interrupt();
            }
        }

        private static void LaunchWebProxy()
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

        private static void LaunchRouting()
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

        private static void LaunchNpmInstall()
        {

                var npmInstallProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = (string)pathToLight,
                        FileName = "npm.cmd",
                        Arguments = "install",
                        RedirectStandardOutput = false
                    }
                };

                npmInstallProcess.Start();
                npmInstallProcess.WaitForExit();   
        }

        private static void LaunchNpmStart()
        {
            var npmStartProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory =pathToLight,
                    FileName = "npm.cmd",
                    Arguments = "start",
                    RedirectStandardOutput = false,
                }
            };

            npmStartProcess.Start();
            Console.WriteLine("The Light Client is running at localhost:3000");
            Console.WriteLine();
            Console.ReadLine();
            Console.WriteLine("The LightClient is closed");
            npmStartProcess.Close();
        }
    }
}