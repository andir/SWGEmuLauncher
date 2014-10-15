using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace SWGPatcher
{
    [Serializable()]
    public class BaseSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        protected void updateVar<T>(ref T var, T value, String name)
        {
            var = value;
            NotifyPropertyChanged(name);
        }
        private Int16 _version;
        public Int16 version
        {
            get
            {
                return _version;
            }
            set
            {
                if (version != value)
                {
                    _version = value;
                    NotifyPropertyChanged("version");
                }

            }
        }
    }
}
