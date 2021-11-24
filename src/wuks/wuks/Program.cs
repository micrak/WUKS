using System.IO;

namespace wuks
{
    using System;
    using System.ServiceProcess;
    using System.Linq;
    using System.Collections.Generic;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.Security.Principal;

    internal static class Program
    {
        public static void Main()
        {
            EventLogger.Initialize();

            if (Environment.UserInteractive)
            {
                RunAsInstaller();
            }
            else
            {
                RunAsService();
            }
        }

        private static void RunAsService()
        {
            try
            {
                EventLogger.Info("Service process started from path " + GetCurrentExecutablePath());
                ServiceBase.Run(new ServiceBase[] { new WuksService() });
            }
            catch (Exception exception)
            {
                EventLogger.Error("Failed to start service process", exception);
            }
        }

        private static void RunAsInstaller()
        {
            PrintInstallerHelp();

            if (!IsAdmin())
            {
                TryRestartAsAdministrator();
                return;
            }

            try
            {
                if (!IsServiceInstalled())
                {
                    Console.WriteLine("Service is not installed.");
                    Console.WriteLine("Press any key to install.");
                    Console.ReadKey();

                    RunInstaller(true);
                }
                else
                {
                    Console.WriteLine("Service is already installed.");
                    Console.WriteLine("Press any key to uninstall.");
                    Console.ReadKey();

                    RunInstaller(false);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Operation failed. " + exception);
            }

            PressAnyKeyToExit();
        }

        private static bool IsServiceInstalled()
        {
            var serviceController = ServiceController.GetServices()
                .FirstOrDefault(g => g.ServiceName == Constants.ServiceName);
            return serviceController != null;
        }

        private static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static string GetCurrentExecutablePath()
        {
            return typeof(Program).Assembly.Location;
        }

        private static void PressAnyKeyToExit()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void RunInstaller(bool install)
        {
            var argsList = new List<string>();
            argsList.Add("/LogFile=");
            argsList.Add("/LogToConsole=true");

            if (!install)
            {
                argsList.Add("/u");
            }

            argsList.Add(GetCurrentExecutablePath());

            ManagedInstallerClass.InstallHelper(argsList.ToArray());
            
            Console.WriteLine();
        }

        private static void PrintInstallerHelp()
        {
            var manual = GetEmbeddedManualContent();
            Console.WriteLine(manual);
            Console.WriteLine();
            Console.WriteLine($"Service name: {Constants.ServiceName}");
            Console.WriteLine($"Executable path: {GetCurrentExecutablePath()}");
            Console.WriteLine();
        }

        private static void TryRestartAsAdministrator()
        {
            Console.WriteLine(
                "Administrator privileges required. Press any key to restart installer with Administrator privileges.");
            Console.ReadKey();

            var processStartInfo = new ProcessStartInfo(GetCurrentExecutablePath());
            processStartInfo.Verb = "runas";

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to restart. {exception.Message}.");
                PressAnyKeyToExit();
            }
        }

        private static string GetEmbeddedManualContent()
        {
            try
            {
                var assembly = typeof(Program).Assembly;
                var stream = assembly.GetManifestResourceStream("wuks.Manual.md");
                if (stream != null)
                {
                    using (stream)
                    {
                        var reader = new StreamReader(stream);
                        return reader.ReadToEnd();
                    }
                }

                return "Failed to open manual. Resource not found.";
            }
            catch(Exception exception)
            {
                return $"Failed to open manual. {exception}.";
            }
        }
    }
}