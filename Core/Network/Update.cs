using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace EnhancedMap.Core.Network
{
    public class Update
    {
        private static WebRequest _WebRequest;

        public static void CheckUpdates()
        {
            if (_WebRequest == null)
                _WebRequest = new WebRequest(new WebClient());
            if (!_WebRequest.Checking)
                _WebRequest.Start();
        }

        private class WebRequest
        {
            private WebClient _client;

            public WebRequest(WebClient client)
            {
                _client = client;
                Checking = false;
            }

            public bool Checking { get; private set; }

            public void Start()
            {
                Console.WriteLine("Checking for update.");

                if (_client == null)
                    _client = new WebClient();

                _client.Proxy = null;
                Checking = true;
                
            }

            private void m_Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
            {
                try
                {
                    Version checkedVersion = new Version(e.Result);

                    if (checkedVersion > MainCore.MapVersion)
                    {
                        Console.WriteLine("New version available: {0}", checkedVersion);

                        var dialogResult = MessageBox.Show($"New version {checkedVersion} is available.\r\n Download now?", "New Update", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            if (File.Exists("EnhancedUpdater.exe"))
                            {
                                Process.Start("EnhancedUpdater.exe");
                                Process.GetCurrentProcess().Kill();
                            }
                            else
                                MessageBox.Show("EnhancedUpdater not found.", "Error");
                        }
                    }
                    else Console.WriteLine("EnhancedMap is already running latest version.");
                }
                catch
                {
                }

                _client.DownloadStringCompleted -= m_Client_DownloadStringCompleted;
                _client?.Dispose();
                _client = null;
                Checking = false;
            }
        }
    }
}