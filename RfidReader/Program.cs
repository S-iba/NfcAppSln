using RfidReader.Data;
using RfidReader.Sensors;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;
using System.Device.Wifi;

namespace RfidReader
{

    public class Program
    {
        // Instance of the RFID reader sensor that handles low-level communication with the RFID hardware
        static RfidReaderSensor _rfIdReader;

        // Stores the most recently read RFID tag data
        static byte[] _lastReadTag;

        static User[] _users = new Seed().SeedData();

        // Set the SSID & Password to your local Wifi network
        const string MYSSID = "Vivo2015";
        const string MYPASSWORD = "debbac5830dc";

        public static void Main()
        {
            try
            {
                // Get the first WiFI Adapter
                WifiAdapter wifi = WifiAdapter.FindAllAdapters()[0];

                // Set up the AvailableNetworksChanged event to pick up when scan has completed
                wifi.AvailableNetworksChanged += Wifi_AvailableNetworksChanged;

                // give it some time to perform the initial "connect"
                // trying to scan while the device is still in the connect procedure will throw an exception
                Thread.Sleep(10_000);

             //   Loop forever scanning every 30 seconds
                while (true)
                {
                    try
                    {
                        Debug.WriteLine("starting Wi-Fi scan");
                        wifi.ScanAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failure starting a scan operation: {ex}");
                    }

                    Thread.Sleep(30000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("message:" + ex.Message);
                Debug.WriteLine("stack:" + ex.StackTrace);
            }

            // Create GPIO controller to manage hardware pins
            GpioController gpioController = new GpioController();

            // Initialize RFID reader with GPIO controller
            _rfIdReader = new RfidReaderSensor(gpioController);
            _rfIdReader.Initialize();

            // Create and start a background thread for continuous RFID reading
            ThreadStart threadStart = new ThreadStart(RfidWork);
            Thread thread = new Thread(threadStart);
            thread.Start();

            // Keep the main thread alive indefinitely
            Thread.Sleep(Timeout.Infinite);
        }

        private static void Wifi_AvailableNetworksChanged(WifiAdapter sender, object e)
        {
            Debug.WriteLine("Wifi_AvailableNetworksChanged - get report");

            // Get Report of all scanned Wifi networks
            WifiNetworkReport report = sender.NetworkReport;

            // Enumerate though networks looking for our network
            foreach (WifiAvailableNetwork net in report.AvailableNetworks)
            {
                // Show all networks found
                Debug.WriteLine($"Net SSID :{net.Ssid},  BSSID : {net.Bsid},  rssi : {net.NetworkRssiInDecibelMilliwatts.ToString()},  signal : {net.SignalBars.ToString()}");

                // If its our Network then try to connect
                if (net.Ssid == MYSSID)
                {
                    // Disconnect in case we are already connected
                    sender.Disconnect();

                    // Connect to network
                    WifiConnectionResult result = sender.Connect(net, WifiReconnectionKind.Automatic, MYPASSWORD);

                    // Display status
                    if (result.ConnectionStatus == WifiConnectionStatus.Success)
                    {
                        Debug.WriteLine("Connected to Wifi network");
                    }
                    else
                    {
                        Debug.WriteLine($"Error {result.ConnectionStatus.ToString()} connecting o Wifi network");
                    }
                }
            }
        }

        // Background worker method that continuously polls for RFID tags

        public static void RfidWork()
        {
            while (true)
            {
                // Read the ID of any RFID/NFC card in range
                var id = _rfIdReader.ReadCardNfcId();
                _lastReadTag = id;

                // Convert to hex if available
                string hex = (id != null && id.Length > 0) ? BitConverter.ToString(id) : "<no id>";


                //Check if the read tag matches any user
                bool userFound = false;
                foreach (var user in _users)
                {
                    if (user.UUID == hex)
                    {
                        userFound = true;
                        Debug.WriteLine($"User recognized: {user.Username}");
                        break;
                    }
                }
                if (userFound == false)
                {
                    Debug.WriteLine("Unknown user");
                }


                // Output to debug
                //Debug.WriteLine(hex);
                Thread.Sleep(1000);
            }
        }
    }
}
