using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace SWGPatcher
{
    class Serializer
    {
        private static XmlSerializer getSerializer<T>()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            if (serializer == null)
            {
                throw new Exception("Failed to craete serializer");
            }
            return serializer;
        }

        public static bool saveToFile<T>(String file, T obj)
        {
            
            if (file == null)
            {
                throw new Exception("file is null");
            }
            
            XmlSerializer serializer = getSerializer<T>();
            using (FileStream fs = File.Open(file, FileMode.OpenOrCreate))
            {
                fs.Seek(0, 0);
                fs.SetLength(0);
                serializer.Serialize(fs, obj);
            }
            
            return true;

        }
        public static T loadFromStream<T>(Stream stream)
        {
            if (stream == null)
            {
                throw new Exception("stream is null.");
            }
            XmlSerializer serializer = getSerializer<T>();

            return (T)serializer.Deserialize(stream);
        }

        public static T loadFromFile<T>(String file)
        {
            if (file == null)
            {
                throw new Exception("File is null");
            }

            using (FileStream fs = new FileStream(file.ToString(), FileMode.Open))
            {
                if (fs == null)
                {
                    throw new Exception("Unknown file?");
                }
                return loadFromStream<T>(fs);
            }
        }
    }
}
