using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.ComponentModel;
namespace SWGPatcher
{
    public delegate void PatcherStatusUpdate(FileRevision fr, uint progress);

    class Patcher
    {
        PatchInformation patchInformation;
        Settings settings;
        public Patcher(PatchInformation patchInformation, Settings settings)
        {
            this.patchInformation = patchInformation;
            this.settings = settings;
        }

        public event PatcherStatusUpdate updateStatus;

        void Run()
        {
            FileRevision[] frs = patchInformation.files;
            if (frs == null)
                return;

            String swgDirectory = settings.swgDirectory;

            if (!Directory.Exists(swgDirectory))
            {
                throw new Exception("SWG Directory does not exist.");
            }

            foreach (FileRevision fr in frs) {
                // check if the file exists, compare hashes, move on from there

                String path = Path.Combine(swgDirectory, fr.file);

                if (File.Exists(path))
                {
                    String hash = calculateHash(fr.hashType, path);
                }
                else
                {
                    patchFile(path, fr);
                }
            }
        }
        public void patchFile(String path, FileRevision fr)
        {
            FileStream fs = File.OpenWrite(path);

            WebClient wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(delegate(object source, AsyncCompletedEventArgs args){
                DownloadCompleted(fr);
            });
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate(object source, DownloadProgressChangedEventArgs args)
            {
                DownloadProgress(fr, args);
            });

        }

        void DownloadCompleted(FileRevision fr)
        {
            updateStatus(fr, 100);
        }

        void DownloadProgress(FileRevision fr, DownloadProgressChangedEventArgs args)
        {
            updateStatus(fr, (uint)args.ProgressPercentage);
        }

        public static String calculateHash(String type, String filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            if (fs == null)
            {
                throw new Exception("Failed to create filestream.");
            }
            byte[] hash = null;
            if (type.ToLower() == "md5")
            {
                MD5 md5 = MD5.Create();

                hash = md5.ComputeHash(fs);
            }
            else
            {
                throw new Exception("Unknown hash algorithm: " + type);
            }

            return arrayToHexString(hash);
        }

        private static String arrayToHexString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in array)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
