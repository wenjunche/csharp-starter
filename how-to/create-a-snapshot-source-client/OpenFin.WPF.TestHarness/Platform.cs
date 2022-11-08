using Newtonsoft.Json.Linq;
using Openfin.Desktop.SnapshotSourceAPI;
using System;
using System.Threading.Tasks;

namespace OpenFin.WPF.TestHarness
{
    internal class Platform
    {
        private ConnectionOptions _connectionOptions;
        private SnapshotSourceClient<Object> _snapshotSourceClient;
        private Object? _snapshot;

        public async Task Connect(string licenseKey, string uuid, string manifestUrl)
        {
            _connectionOptions = new ConnectionOptions(licenseKey, uuid);
            _connectionOptions.ConnectedRuntime.Error += (sender, e) =>
            {
                Console.Write(e);
            };

            _connectionOptions.ConnectedRuntime.Connect(async () =>
            {
                try
                {
                    var platform = await _connectionOptions.ConnectedRuntime.System.LaunchManifestAsync<JObject>(manifestUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error trying to launch manifest: " + manifestUrl, ex);
                }
            });
        }

        public async Task GetSnapshot(string platformUUID)
        {
            try { 
                _snapshotSourceClient = await _connectionOptions.ConnectedRuntime.SnapshotSource.CreateSnapshotSourceClientAsync<Object>(platformUUID);
                _snapshot = await _snapshotSourceClient.GetSnapshotAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting snapshot", ex);
            }
        }

        public async Task ApplySnapshot()
        {
            if (_snapshotSourceClient != null && _snapshot != null)
            {
                try
                {
                    await _snapshotSourceClient.ApplySnapshotAsync(_snapshot);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting snapshot", ex);
                }
            }
        }
    }
}
