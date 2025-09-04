using RfidReader.Models;
using RfidReader.Services;
using System;
using System.Diagnostics;
using System.Threading;

namespace RfidReader
{
    public class Program
    {
        private static RfidCardReader _cardReader;
        public static void Main()
        {
           

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T

            Debug.WriteLine("=== Simple RFID UID Reader ===");
            Debug.WriteLine("ESP32 + RC522 + .NET nanoFramework");
            Debug.WriteLine("Reading UID only - Simplified version");
            Debug.WriteLine("==================================");

            try
            {
                // Initialize the card reader
                _cardReader = new RfidCardReader();

                // Subscribe to events
                _cardReader.CardDetected += OnCardDetected;
                _cardReader.ErrorOccured += OnErrorOccurred;

                // Initialize hardware
                if (!_cardReader.Initialize())
                {
                    Debug.WriteLine("Failed to initialize RFID reader. Check hardware connections.");
                    Debug.WriteLine("Verify wiring according to the setup guide.");
                    return;
                }

                Debug.WriteLine("RFID reader initialized successfully!");
                Debug.WriteLine($"Status: {_cardReader.GetStatus()}");
                Debug.WriteLine("");
                Debug.WriteLine(">>> Place an RFID card near the reader to read its UID <<<");
                Debug.WriteLine("");

                // Start scanning for cards
                _cardReader.StartScanning();

                // Keep the application running
                RunMainLoop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fatal error: {ex.Message}");
            }
            finally
            {
                // Clean up resources
                _cardReader?.Dispose();
                Debug.WriteLine("Application terminated.");
            }
        }

        private static void RunMainLoop()
        {
            var lastStatusCheck = DateTime.UtcNow;
            var cardCount = 0;

            while (true)
            {
                try
                {
                    // Show status every 60 seconds
                    if (DateTime.UtcNow - lastStatusCheck > TimeSpan.FromSeconds(60))
                    {
                        Debug.WriteLine($"[INFO] Reader Status: {_cardReader.GetStatus()}");
                        Debug.WriteLine($"[INFO] Cards read so far: {cardCount}");
                        Debug.WriteLine($"[INFO] Waiting for cards... {DateTime.UtcNow:HH:mm:ss}");
                        lastStatusCheck = DateTime.UtcNow;
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in main loop: {ex.Message}");
                    Thread.Sleep(5000);
                }
            }

        }

        private static void OnCardDetected(object sender, CardInfo cardInfo)
        {
            Debug.WriteLine("");
            Debug.WriteLine("╔══════════════════════════════════╗");
            Debug.WriteLine("║           CARD DETECTED          ║");
            Debug.WriteLine("╠══════════════════════════════════╣");
            Debug.WriteLine($"║ UID (Hex):    {cardInfo.UidString.PadRight(18)} ║");
            Debug.WriteLine($"║ UID (Dec):    {cardInfo.UidDecimal.PadRight(18)} ║");
            Debug.WriteLine($"║ UID Length:   {cardInfo.Uid.Length} bytes{string.Empty.PadRight(13)} ║");
            Debug.WriteLine($"║ UID Type:     {CardDataParser.GetUidType(cardInfo.Uid).PadRight(18)} ║");
            Debug.WriteLine($"║ Read Time:    {cardInfo.ReadTime:HH:mm:ss}.{cardInfo.ReadTime.Millisecond:000}{string.Empty.PadRight(8)} ║");

            // Show integer value for 4-byte UIDs
            var uidInt = CardDataParser.UidToInteger(cardInfo.Uid);
            if (uidInt.ToString() != null)
            {
                Debug.WriteLine($"║ UID (Int):    {uidInt.ToString().PadRight(18)} ║");
            }

            Debug.WriteLine("╚══════════════════════════════════╝");

            // Clean formatted output for easy copying
            Debug.WriteLine("");
            Debug.WriteLine($"Quick Copy: {CardDataParser.FormatUidForDisplay(cardInfo.Uid)}");
            Debug.WriteLine("");
        }

        private static void OnErrorOccurred(object sender, string error)
        {
            Debug.WriteLine($"[ERROR] {DateTime.UtcNow:HH:mm:ss} - {error}");
        }

        private static void TestSingleRead()
        {
            Debug.WriteLine("=== Manual Card Read Test ===");

            if (_cardReader.IsCardPresent())
            {
                Debug.WriteLine("Card detected, reading UID...");
                var cardInfo = _cardReader.ReadSingleCard();
                if (cardInfo != null)
                {
                    OnCardDetected(null, cardInfo);
                }
                else
                {
                    Debug.WriteLine("Failed to read card UID");
                }
            }
            else
            {
                Debug.WriteLine("No card present. Place a card near the reader and try again.");
            }

            Debug.WriteLine("=============================");
        }
    }
}

  
