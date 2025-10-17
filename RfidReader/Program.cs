using RfidReader.Sensors;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace RfidReader
{

    public class Program
    {
        // Instance of the RFID reader sensor that handles low-level communication with the RFID hardware
        static RfidReaderSensor _rfIdReader;

        // Stores the most recently read RFID tag data
        static byte[] _lastReadTag;

        public static void Main()
        {
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

                // Output to debug
                Debug.WriteLine(hex);
                Thread.Sleep(1000);
            }
        }
    }
}
