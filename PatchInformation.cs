using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace SWGPatcher
{

    public struct FileRevision
    {
        public String file;
        public String hash;
        public String hashType;
    }

    public class PatchInformation
    {
        public static event PatchInformationLoadProgressHandler PatchInformationLoadProgress;
        public static event PatchInformationLoadedHandler PatchInformationLoaded;
        public static event PatchInformationLoadFailedHandler PatchInformationLoadFailed;

        public UInt16 version;
        public FileRevision[] files;
        public String uri;

        public static async Task downloadPatchInformation(String uri)
        {
            if (uri == null)
            {
                throw new Exception("uri was null");
            }

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                {
                    if (PatchInformationLoadProgress != null)
                        PatchInformationLoadProgress(e.ProgressPercentage);
                };
                wc.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
                {
                    if (e.Error != null)
                    {
                        if (PatchInformationLoadFailed != null)
                            PatchInformationLoadFailed(e.Error.ToString());
                        return;
                    }
                    String data = e.Result;
                    String signature = Crypto.getSignature(data);
                    if (signature != null)
                    {

                    }
                    else if (signature == null && GlobalAppData.settings.enforceSignature == true)
                    {
                        if (PatchInformationLoadFailed != null)
                            PatchInformationLoadFailed("Couldn't find required signature.");
                        return;
                    }

                    PatchInformation pi = _deserializePatchInformation(e.Result);

                    if (PatchInformationLoaded != null)
                        PatchInformationLoaded(pi);

                };
                try
                {
 
                    try
                    {
                        wc.DownloadStringTaskAsync(uri);
                    }
                    catch (System.Net.WebException)
                    {
                        // this is being handeld in the event handler above already
                    }
                    catch (ArgumentException e)
                    {
                        if (PatchInformationLoadFailed != null)
                            PatchInformationLoadFailed("Uri is invalid: '" + uri + "'\n" + e.ToString());
                        return;
                    }
                }
                catch (Exception e)
                {
                    if (PatchInformationLoadFailed != null)
                        PatchInformationLoadFailed(e.ToString());
                }
            }

        }
        
        private static PatchInformation _deserializePatchInformation(String data) {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(data);
                writer.Flush();
                stream.Position = 0;

                PatchInformation pi = Serializer.loadFromStream<PatchInformation>(stream);
                stream.Close();
                return pi;
        }

    }

    public delegate void PatchInformationLoadProgressHandler(int progress);
    public delegate void PatchInformationLoadedHandler(PatchInformation pi);
    public delegate void PatchInformationLoadFailedHandler(String reason);





}
