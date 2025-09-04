using NfcAppTrial.Constants;
using NfcAppTrial.Hardware;
using NfcAppTrial.Services;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
//using static System.Debug;

namespace NfcAppTrial
{
    public class Program
    {   
        private static RfidReaderService _rfidService;
        private static bool _isShuttingDown = false;

        static  void Main(string[] args)
        {
            Debug.WriteLine("Starting RFID Reader Application...");
            Debug.WriteLine("Press Ctrl+C to exit");

            // Handle graceful shutdown
             //+= OnCancelKeyPress;

            try
            {
                // Initialize hardware controllers
                var gpioController = new NfcAppTrial.Hardware.GpioController();
                var spiController = new SpiController();

                gpioController.Initialize();

                // Initialize MFRC522
                var mfrc522 = new MFRC522(
                    PinConfig.SS_PIN,
                    PinConfig.RST_PIN,
                    spiController,
                    gpioController);

                // Initialize services
                var ledController = new LedController(gpioController);
                _rfidService = new RfidReaderService(mfrc522, ledController);

                // Subscribe to events
                _rfidService.CardDetected += OnCardDetected;
                _rfidService.CardError += OnCardError;

                // Initialize and start the service
                _rfidService.Initialize();
                _rfidService.Start();

                // Run the card processing loop
                 //Run(() => _rfidService.ProcessCards());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                Cleanup();
                Debug.WriteLine("Application shutdown complete");
            }
           
        }

        private static void OnCardDetected(object sender, CardDetectedEventArgs e)
        {
            Debug.WriteLine($"[{e.Timestamp:HH:mm:ss}] Card detected: {e.UidString}");

            // Add your custom card processing logic here
            ProcessCard(e.UidString);
        }

        private static void OnCardError(object sender, CardErrorEventArgs e)
        {
            Debug.WriteLine($"[{e.Timestamp:HH:mm:ss}] Card error: {e.ErrorMessage}");
        }

        private static void ProcessCard(string cardUid)
        {
            // Example: Check if card is authorized
            if (IsAuthorizedCard(cardUid))
            {
                Debug.WriteLine($"Access granted for card: {cardUid}");
                // Add access granted logic here
            }
            else
            {
                Debug.WriteLine($"Access denied for card: {cardUid}");
                // Add access denied logic here
            }
        }

        private static bool IsAuthorizedCard(string cardUid)
        {
            // Example authorized cards - replace with your logic
            string[] authorizedCards = {
                "04A1B2C3D4E5F6",
                "04B1C2D3E4F5A6",
                "04C1D2E3F4A5B6"
            };

            foreach (var authorizedCard in authorizedCards)
            {
                if (cardUid.StartsWith(authorizedCard))
                {
                    return true;
                }
            }

            return false;
        }

        private static void OnCancelKeyPress(object sender)
        {
            Debug.WriteLine("\nShutdown requested...");
            //e.Cancel = true; // Prevent immediate termination
            _isShuttingDown = true;

            _rfidService?.Stop();
        }

        private static void Cleanup()
        {
            try
            {
                _rfidService?.Stop();
                Debug.WriteLine("Services stopped");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }
    }
}
