using NfcAppTrial.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NfcAppTrial.Services
{
    public class RfidReaderService
    {
        private readonly IMFRC522 _mfrc522;
        private readonly LedController _ledController;
        private byte[] _authenticationKey;
        private bool _isRunning = false;

        public event EventHandler<CardDetectedEventArgs> CardDetected;
        public event EventHandler<CardErrorEventArgs> CardError;

        public RfidReaderService(IMFRC522 mfrc522, LedController ledController)
        {
            _mfrc522 = mfrc522;
            _ledController = ledController;
            _authenticationKey = new byte[6] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }; // Default MIFARE key
        }

        public void Initialize()
        {
            Console.WriteLine("Initializing RFID Reader Service...");

            _mfrc522.PCD_Init();

            Console.WriteLine("Approach your reader card...");
            Console.WriteLine();
        }

        public void Start()
        {
            _isRunning = true;
            Console.WriteLine("RFID Reader Service started");
        }

        public void Stop()
        {
            _isRunning = false;
            _ledController.TurnOffAll();
            Console.WriteLine("RFID Reader Service stopped");
        }

        public void ProcessCards()
        {
            while (_isRunning)
            {
                try
                {
                    if (_mfrc522.PICC_IsNewCardPresent())
                    {
                        if (_mfrc522.PICC_ReadCardSerial())
                        {
                            var uid = _mfrc522.GetCardUid();
                            HandleCardDetected(uid);
                        }
                        else
                        {
                            HandleCardError("Failed to read card serial");
                        }
                    }
                }
                catch (Exception ex)
                {
                    HandleCardError($"Exception during card processing: {ex.Message}");
                }

                Thread.Sleep(100); // Prevent excessive CPU usage
            }
        }

        private void HandleCardDetected(byte[] uid)
        {
            string uidString = BitConverter.ToString(uid);
            Console.WriteLine($"Card detected with UID: {uidString}");

            _ledController.TurnOnGreen();

            // Raise event
            CardDetected?.Invoke(this, new CardDetectedEventArgs(uid, uidString));

            Thread.Sleep(2000); // Keep LED on for 2 seconds
            _ledController.TurnOffAll();
        }

        private void HandleCardError(string errorMessage)
        {
            Console.WriteLine($"Card error: {errorMessage}");

            _ledController.TurnOnRed();

            // Raise event
            CardError?.Invoke(this, new CardErrorEventArgs(errorMessage));

            Thread.Sleep(1000);
            _ledController.TurnOffAll();
        }

        public void SetAuthenticationKey(byte[] key)
        {
            if (key.Length != 6)
                throw new ArgumentException("Authentication key must be 6 bytes");

            _authenticationKey = key;
        }
    }

    public class CardDetectedEventArgs : EventArgs
    {
        public byte[] Uid { get; }
        public string UidString { get; }
        public DateTime Timestamp { get; }

        public CardDetectedEventArgs(byte[] uid, string uidString)
        {
            Uid = uid;
            UidString = uidString;
            Timestamp = DateTime.UtcNow;
        }
    }

    public class CardErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; }
        public DateTime Timestamp { get; }

        public CardErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Timestamp = DateTime.UtcNow;
        }
    }
}
