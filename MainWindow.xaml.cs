using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Management;
using System.ComponentModel;

namespace SWGPatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Settings settings = GlobalAppData.settings;
            if (settings != null)
            {
                DataContext = settings;
                settings.PropertyChanged += settingsPropertyChanged;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Cannot obtain settings.");
                this.Close();
            }
        }
        private void settingsPropertyChanged(Object source, PropertyChangedEventArgs args) {
            String propertyName = args.PropertyName;
            switch (propertyName)
            {
                case "swgDirectory":
                        //TODO: check installation and ask if it should proceed with patching
                        Settings s = source as Settings;
                        
                    break;
                default:
                    return;
            }
            
            


        }
        private void launchButtonClicked(object sender, RoutedEventArgs e)
        {
            if (launchButton.IsEnabled == false) return;
            Settings s = GlobalAppData.settings;
            String executablePath = System.IO.Path.Combine(s.swgDirectory, s.executableName);
            GameProcess proc = new GameProcess(executablePath);
            using (var managementClass = new ManagementClass("Win32_Process"))
            {
                var processInfo = new ManagementClass("Win32_ProcessStartup");
                processInfo.Properties["CreateFlags"].Value = 0x00000008;

                var inParameters = managementClass.GetMethodParameters("Create");
                inParameters["CommandLine"] = executablePath;
                inParameters["ProcessStartupInformation"] = processInfo;

                var result = managementClass.InvokeMethod("Create", inParameters, null);
                if ((result != null) && ((uint)result.Properties["ReturnValue"].Value != 0))
                {
                    Console.WriteLine("Process ID: {0}", result.Properties["ProcessId"].Value);
                }
            }

        }

        private void setDirectoryButtonClicked(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = GlobalAppData.settings.swgDirectory;
           
            
            DialogResult res = dialog.ShowDialog();

            if (System.Windows.Forms.DialogResult.OK == res)
            {
                //OK
                GlobalAppData.settings.swgDirectory = dialog.SelectedPath;
            }
            else
            {
                //cancel
            }
        }
    }
}
