using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using WebProxyService;
using IService1 = Routing.IService1;

namespace Host
{
    internal class Program
    {
        private static readonly string pathToLight =
            Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName).Parent
                .FullName + "\\LightClient\\ClientApp";
        private static string pathToExecutable = @"../../../HeavyClient/bin/Debug/net6.0-windows";


        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        private static void Main(string[] args)
        {
            var tProxy = new Thread(LaunchWebProxy);
            var tRouting = new Thread(LaunchRouting);
            var tNpmInstallAndBuild = new Thread(LaunchNpmInstallAndBuild);
            var tNpmStart = new Thread(LaunchNpmStart);
            var tNpmRunDev = new Thread(LaunchNpmRunDev);

            Thread heavyClientThread = new Thread(() =>
            {
                ExecuteExternalExecutable(pathToExecutable, "HeavyClient.exe");
            });

            tProxy.Priority = ThreadPriority.Highest;
            tRouting.Priority = ThreadPriority.AboveNormal;
            tNpmStart.Priority = ThreadPriority.Normal;

            tProxy.Start();
            tRouting.Start();

            if (tRouting.IsAlive && tProxy.IsAlive)
            {
                heavyClientThread.Start();
            }

            if (!Directory.Exists(Path.Combine(pathToLight, "node_modules")))
            {
                tNpmInstallAndBuild.Start();
                tNpmInstallAndBuild.Join();
            }

            if (tNpmInstallAndBuild.IsAlive == false)
            {
                tNpmRunDev.Start();
            }

            tProxy.Join();
            tRouting.Join();
            heavyClientThread.Join();
            tNpmRunDev.Join();
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

        private static void LaunchNpmInstallAndBuild()
        {
            var npmProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToLight,
                    FileName = "npm.cmd",
                    RedirectStandardOutput = false
                }
            };

            npmProcess.StartInfo.Arguments = "install --force";
            npmProcess.Start();
            Thread.Sleep(500);
            SetWindowText(npmProcess.MainWindowHandle, "NPM install");
            npmProcess.WaitForExit();

            if(!Directory.Exists(Path.Combine(pathToLight, "build")))
            {
                npmProcess.StartInfo.Arguments = "run build";
                npmProcess.Start();
                Thread.Sleep(500);
                SetWindowText(npmProcess.MainWindowHandle, "NPM build");
                npmProcess.CloseMainWindow();
            }
        }


        private static void LaunchNpmStart()
        {
            var npmStartProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToLight,
                    FileName = "npm.cmd",
                    Arguments = "start",
                    RedirectStandardOutput = false,
                },

            };

            npmStartProcess.Start();
            Thread.Sleep(500);
            SetWindowText(npmStartProcess.MainWindowHandle, "Light Client React");
            Console.WriteLine("The Light Client is running [Start] at localhost:3000");
            Console.WriteLine("Press <Enter> to terminate the service.");
            Console.WriteLine();
            Console.ReadLine();
            Console.WriteLine("The LightClient is closed");
            npmStartProcess.Close();
        }

        private static void LaunchNpmRunDev()
        {
            var npmRunDevProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToLight,
                    FileName = "serve",
                    Arguments = "-s build",
                    RedirectStandardOutput = false,
                },
            };

            npmRunDevProcess.Start();
            Thread.Sleep(500);
            SetWindowText(npmRunDevProcess.MainWindowHandle, "Light Client React");
            Console.WriteLine("The Light Client is running [DEV] at localhost:3000");
            Console.WriteLine("Press <Enter> to terminate the service.");
            Console.WriteLine();
            Console.ReadLine();
            Console.WriteLine("The LightClient [DEV] is closed");
            npmRunDevProcess.Close();
        }

        private static void ExecuteExternalExecutable(string directoryPath, string executableName)
        {
            string fullPathToExecutable = System.IO.Path.Combine(directoryPath, executableName);

            if (System.IO.File.Exists(fullPathToExecutable))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = directoryPath,
                    FileName = executableName,
                };

                try
                {
                    // Start the process
                    Process process = new Process { StartInfo = startInfo };
                    process.Start();
                    Console.WriteLine("The Heavy Client [WPF] is running");
                    Console.WriteLine("Press <Enter> to terminate the service.");
                    Console.WriteLine();
                    Console.ReadLine();
                    Console.WriteLine("The Heavy Client [WPF] is closed");
                    process.Kill();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("The specified executable file does not exist.");
            }
        }
    }
}