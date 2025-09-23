using Iot.Device.Mfrc522;
using Iot.Device.Rfid;
using nanoFramework.Hardware.Esp32;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Spi;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace RfidReader.Sensors
{
    public class RfidReaderSensor
    {
        private GpioController _gpioController;
        private MfRc522 _mfRc522;

        public RfidReaderSensor(GpioController gpioController)
        {
            _gpioController = gpioController;
        }

        public void Initialize()
        {
            int pinReset = 22;

            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(23, DeviceFunction.SPI1_MOSI);
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(19, DeviceFunction.SPI1_MISO);
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(18, DeviceFunction.SPI1_CLOCK);

            SpiConnectionSettings connection = new(1, 21);
            connection.ClockFrequency = 5_000_000;

            SpiDevice spi = SpiDevice.Create(connection);
            _mfRc522 = new(spi, pinReset, _gpioController, false);
        }

        public byte[] ReadCardNfcId()
        {
            bool res;
            Data106kbpsTypeA card;
            do
            {
                res = _mfRc522.ListenToCardIso14443TypeA(out card, TimeSpan.FromSeconds(2));
                Thread.Sleep(res ? 0 : 200);
            }
            while (!res);

            if (card != null)
            {
                Debug.WriteLine("Card Found");
                
                return card.NfcId;

            }
            else
            {
                Debug.WriteLine("No card found");

                return null;
            }
        }

    }
}
