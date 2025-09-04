using Iot.Device.Mfrc522;
using RfidReader.Constants;
using RfidReader.Models;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace RfidReader.Services
{
    public class RfidCardReader : IDisposable
    {
      
            public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _rfid?.Dispose();
                _spiDevice?.Dispose();
                _gpio?.Dispose();

                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine("RFID reader disposed");
                }
            }
            catch (Exception ex)
            {
                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine($"Error disposing RFID reader: {ex.Message}");
                }
            }
        }

        private MfRc522 _rfid;
        private SpiDevice _spiDevice;
        private GpioController _gpio;
        private bool _disposed = false;
        private string _lastUidRead = string.Empty;

        public event EventHandler<CardInfo> CardDetected;
        public event EventHandler<string> ErrorOccured;

        public bool Initialize()
        {
            try
            {
                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine("Initializing RFID reader...");
                }

                // Initialize GPIO Controller
                _gpio = new GpioController();

                //Setup SPI config
                var spiSettings = new SpiConnectionSettings(Configs.SPI_BUS, Configs.SDA_PIN)
                {
                    ClockFrequency = Configs.CLOCK_FREQUENCY,
                    Mode = SpiMode.Mode0,
                    DataBitLength = 8
                };

                // SPI device
                _spiDevice = SpiDevice.Create(spiSettings);

                //Initialize MFRC522
                _rfid = new MfRc522(_spiDevice,Configs.RST_PIN,_gpio,false);

                //Test Communication
                var version = _rfid.Version;
                if (Configs.ENABLE_DEBUG_OUTPUT) 
                {
                    Debug.WriteLine($"MFRC522 version: 0x{version:X2}");


                }

                if (version.ToString() == "0x00" || version.ToString() == "0xFF")
                {
                    var error = "Failed to communicate with MFRC522. Check wiring.";
                    if (Configs.ENABLE_DEBUG_OUTPUT)
                    {
                        Debug.WriteLine(error);
                    }
                    ErrorOccured?.Invoke(this, error);
                    return false;
                }

                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine("RFID reader initialized successfully");
                }

                return true;
            }
            catch (Exception ex)
            {

                var error = $"Failed to initialize RFID reader: {ex.Message}";
                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine(error);
                }
                ErrorOccured?.Invoke(this, error);
                return false;
            }
        }

        public void StartScanning()
        {
            if (_rfid == null)
            {
                ErrorOccured?.Invoke(this, "RFID reader not initialized");
                return;
            }

            if (Configs.ENABLE_DEBUG_OUTPUT)
            {
                Debug.WriteLine("Starting UID scanning...");
                Debug.WriteLine("Place an RFID card near the reader...");
            }

            var scanThread = new Thread(ScanForCards);
            scanThread.Start();
        }

        private void ScanForCards()
        {
            while (!_disposed)
            {
                try
                {
                    if (IsCardPresent())
                    {
                        var cardInfo = ReadCardUid();
                        if (cardInfo != null)
                        {
                            // Only trigger event if it's a different card
                            if (cardInfo.UidString != _lastUidRead)
                            {
                                _lastUidRead = cardInfo.UidString;
                                CardDetected?.Invoke(this, cardInfo);

                                if (Configs.ENABLE_DEBUG_OUTPUT)
                                {
                                    Debug.WriteLine($"New card UID: {cardInfo.UidString}");
                                }
                            }

                            // Small delay to avoid rapid re-reading
                            Thread.Sleep(500);
                        }
                    }
                    else
                    {
                        // Clear last read UID when no card is present
                        if (!string.IsNullOrEmpty(_lastUidRead))
                        {
                            _lastUidRead = string.Empty;
                            if (Configs.ENABLE_DEBUG_OUTPUT)
                            {
                                Debug.WriteLine("Card removed - ready for next card");
                            }
                        }
                    }

                    Thread.Sleep(Configs.CARD_SCAN_INTERVAL_MS);
                }
                catch (Exception ex)
                {
                    if (Configs.ENABLE_DEBUG_OUTPUT)
                    {
                        Debug.WriteLine($"Error during card scanning: {ex.Message}");
                    }
                    ErrorOccured?.Invoke(this, ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        public bool IsCardPresent()
        {
            try
            {
                byte[] atqa = new byte[2];

                return _rfid.IsCardPresent(atqa) == true;
            }
            catch
            {

                return false;
            }
        }

        public CardInfo ReadCardUid()
        {
            try
            {
                if (!IsCardPresent())
                    return null;

                // Get the UID without reading card data
                var cardData = _rfid.ListenToCardIso14443TypeA(out var uid, TimeSpan.FromMilliseconds(1000));

                if (uid == null || uid.ToString().Length == 0)
                {
                    return null;
                }

                // Validate UID
                if (!CardDataParser.IsValidUid(uid.NfcId))
                {
                    if (Configs.ENABLE_DEBUG_OUTPUT)
                    {
                        Debug.WriteLine($"Invalid UID format: {uid.ToString().Length} bytes");
                    }
                    return null;
                }

                // Create card info with just the UID
                var cardInfo = CardDataParser.ParseCard(uid.NfcId);

                // Stop communication with the card
                _rfid.Halt();

                return cardInfo;
            }
            catch (Exception ex)
            {
                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine($"Error reading UID: {ex.Message}");
                }
                return null;
            }
        }

        public CardInfo ReadSingleCard()
        {
            try
            {
                if (Configs.ENABLE_DEBUG_OUTPUT)
                {
                    Debug.WriteLine("Reading single card...");
                }

                return ReadCardUid();
            }
            catch (Exception ex)
            {
                ErrorOccured?.Invoke(this, ex.Message);
                return null;
            }
        }

        public string GetStatus()
        {
            try
            {
                if (_rfid == null)
                    return "RFID reader not initialized";

                var version = _rfid.Version;
                var isCardPresent = IsCardPresent();

                return $"MFRC522 v0x{version:X2}, Card present: {isCardPresent}";
            }
            catch (Exception ex)
            {
                return $"Status error: {ex.Message}";
            }
        }

    }
}
