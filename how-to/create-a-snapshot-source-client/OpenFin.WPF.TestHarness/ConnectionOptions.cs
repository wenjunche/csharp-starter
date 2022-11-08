using Openfin.Desktop;
using System;

namespace OpenFin.WPF.TestHarness
{
    public class ConnectionOptions
    {
        public ConnectionOptions(string licenseKey, string uuid)
        {
            var runtimeOptions = new RuntimeOptions()
            {
                UUID = uuid,
                Version = "stable",
                LicenseKey = licenseKey
            };
            this.UUID = uuid;
            this.ConnectToRuntime(runtimeOptions);
        }
        public ConnectionOptions(RuntimeOptions runtimeOptions)
        {
            this.ConnectToRuntime(runtimeOptions);
        }

        public ConnectionOptions(Runtime runtime)
        {
            this.ConnectedRuntime = runtime;
        }

        public string UUID { get; private set; }
        public Runtime ConnectedRuntime { get; set; }

        public event EventHandler RuntimeConnected;

        public event EventHandler RuntimeDisconnected;

        private void Runtime_Disconnected(object sender, EventArgs e)
        {
            RuntimeDisconnected?.Invoke(this, EventArgs.Empty);
        }
        private void Runtime_Connected(object sender, EventArgs e)
        {
            RuntimeConnected?.Invoke(this, EventArgs.Empty);
        }

        private void ConnectToRuntime(RuntimeOptions runtimeOptions)
        {
            this.ConnectedRuntime = Runtime.GetRuntimeInstance(runtimeOptions);
            this.ConnectedRuntime.Connected += Runtime_Connected;
            this.ConnectedRuntime.Disconnected += Runtime_Disconnected;
        }
    }
}
