using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Reflection;
namespace SWGPatcher
{
   

    [Serializable()]
    public class Settings : BaseSettings
    {
        public Settings()
        {
            version = 1;

            this._patcherURI = "http://bloodfin.rammhold.de/patcher.xml";
            this._swgDirectory = "";
            this._executableName = "Bloodfin.exe";
            this._enforceSignature = false;
        }



        private String _patcherURI;
        public String patcherURI
        {
            get
            {
                return _patcherURI;
            }
            set
            {
                updateVar(ref _patcherURI, value, "patcherURI");
            }
        }
        private String _swgDirectory;
        public String swgDirectory
        {
            get
            {
                return _swgDirectory;
            }
            set
            {
                updateVar(ref _swgDirectory, value, "swgDirectory");
            }
        }
        private String _executableName;
        public String executableName {
            get {
                return _executableName;
            }
            set {
                updateVar(ref _executableName, value, "executableName");
            }
        }
        private bool _enforceSignature;
        public bool enforceSignature
        {
            get
            {
                return _enforceSignature;
            }
            set
            {
                updateVar(ref _enforceSignature, value, "enforceSignature");
            }
        }
    }
    public delegate void SettingsLoadedHandler(Settings settings);
}
