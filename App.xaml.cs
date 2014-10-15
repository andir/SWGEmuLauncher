using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.IO;
namespace SWGPatcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public event CriticalFailureHandler CriticalFailureEvent;
        public App() : base()
        {
            this.ShutdownMode = ShutdownMode.OnLastWindowClose;
            this.CriticalFailureEvent += (reason) =>
            {
                MessageBox.Show("Critical failure. Shutting down. \n" + reason);
                doShutdown();
            };
            PatchInformation.PatchInformationLoadFailed += (reason) =>
            {
                MessageBox.Show("Downloading patch information failed: " + reason);
                doShutdown();
               
            };

            GlobalAppData.SettingsLoaded += (settings) =>
            {
                Task t = PatchInformation.downloadPatchInformation(settings.patcherURI);
            };

            GlobalAppData.initalize(this);
            
        }
        private void doShutdown()
        {
            ThreadStart ts = delegate()
            {
                Dispatcher.BeginInvoke((Action)delegate()
                {
                    Application.Current.Shutdown();
                });
            };
            Thread t = new Thread(ts);
            t.Start();
        }
        public delegate void CriticalFailureHandler(String reason);
    }
   
}
