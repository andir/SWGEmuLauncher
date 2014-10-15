using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                this.Shutdown(1);
            };
            PatchInformation.PatchInformationLoadFailed += (reason) =>
            {
                MessageBox.Show("Downloading patch information failed: " + reason);
                try // this is silly.. why does it crash with InvalidOperation on shutdown?
                {
                    this.Shutdown(1);
                }
                catch (InvalidOperationException)
                {
                    //TODO
                }
               
            };

            GlobalAppData.SettingsLoaded += (settings) =>
            {
                Task t = PatchInformation.downloadPatchInformation(settings.patcherURI);
            };

            GlobalAppData.initalize(this);
            
        }
        public delegate void CriticalFailureHandler(String reason);
    }
   
}
