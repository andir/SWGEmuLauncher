using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Net;

namespace SWGPatcher
{
    public class GlobalAppData
    {

        public static App app;
        public static Settings settings;

        public static event SettingsLoadedHandler SettingsLoaded;
        public static PatchInformation patchInformation;

        private static String getUserProfilePath()
        {
            String userProfilePath = Path.GetFullPath(Environment.GetEnvironmentVariable("USERPROFILE"));
            String p = Path.Combine(userProfilePath, "SWGPatcher");
            return p;
        }

        static String settingsPath = null;
        static string settingsFile
        {
            get
            {
                return Path.Combine(getUserProfilePath(), "settings.xml");
            }
        }

        public static bool initalize(App app)
        {
            GlobalAppData.patchInformation = null;
            GlobalAppData.app = app;
            settingsPath = getUserProfilePath();
            if (settings == null)
            {
                String f = settingsFile;
                String d = settingsPath;

                if (!Directory.Exists(settingsPath))
                {
                    DirectoryInfo dInfo = Directory.CreateDirectory(settingsPath);
                    if (!dInfo.Exists)
                    {
                        MessageBox.Show("Failed to create profile directory: " + settingsPath);
                        return false;
                    }
                }
                loadSettings();

                return true;
            }
            return true;
        }
        public static bool loadSettings()
        {
            if (!File.Exists(settingsFile))
            {
                settings = new Settings();
            }
            else
            {

                try
                {

                    settings = Serializer.loadFromFile<Settings>(settingsFile);
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show(ioe.ToString());
                    MessageBoxResult res = MessageBox.Show("Failed to load settings. Create new?", "Failed to load settings", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        settings = new Settings();
                    }
                    else
                    {
                        MessageBox.Show("Can't continue without database. Exiting.");
                        return false;
                    }
                }
            }
            if (settings != null)
                app.Exit += storeSettings;
            else
            {
                MessageBox.Show("Failed to create settings object.");
                return false;
            }
            if (SettingsLoaded != null)
                SettingsLoaded(settings);
            return true;
        }
        private static void storeSettings(object sender, ExitEventArgs e)
        {
            if (settings != null)
            {
                try
                {
                    Serializer.saveToFile<Settings>(settingsFile, settings);
                } 
                catch(System.IO.IOException ioe)
                {
                    MessageBox.Show("Failed to save settings: " + ioe.ToString());
                }
            }
        }
    }
}
