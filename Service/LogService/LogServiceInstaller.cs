using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Win32;

namespace LogService
{
    [RunInstaller(true)]
    public class LogServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public LogServiceInstaller()
        {
            Installers.Add(service = new ServiceInstaller()
            {
                ServiceName = LogServiceInstaller.InstallServiceName,
                DisplayName = LogServiceInstaller.InstallServiceName,
                StartType = ServiceStartMode.Automatic,
            });

            Installers.Add(process = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalSystem,
            });
        }

        [AppSetting("ServiceName"), DefaultValue("LogService")]
        internal static string InstallServiceName
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
            set { }
        }

        internal static bool IsServiceInstalled
        {
            get
            {
                foreach (ServiceController svc in ServiceController.GetServices())
                    if (svc.ServiceName == LogServiceInstaller.InstallServiceName)
                        return true;
                return false;
            }
        }

        internal static void InstallService()
        {
            if (IsServiceInstalled)
                UninstallService();
            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
            // Here is where we set the bit on the value in the registry.
            // Grab the subkey to our service
            RegistryKey ckey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + LogServiceInstaller.InstallServiceName, true);
            // Good to always do error checking!
            if (ckey != null)
            {
                // Ok now lets make sure the "Type" value is there, 
                //and then do our bitwise operation on it.
                if (ckey.GetValue("Type") != null)
                {
                    ckey.SetValue("Type", ((int)ckey.GetValue("Type") | 256));
                }
            }
        }

        internal static void UninstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
        }
    }
}
