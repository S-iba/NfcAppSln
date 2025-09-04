using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;

namespace RfidReader.Constants
{
    public class Configs
    {
        public const int SPI_BUS = 1; //SPI1 on ESP32
        public const int CLOCK_FREQUENCY = 1000000;//1MHz 

        //GPIO Pin Constants
        public const int RST_PIN = 22;
        public const int SDA_PIN = 5;
        public const int MOSI_PIN = 23;
        public const int MISO_PIN = 19;
        public const int SCK_PIN = 18;

        //GPIO Pin Modes
        public const PinMode RST_PIN_MODE = PinMode.Output;

        //RFID Settings
        public const int CARD_READ_TIMEOUT_MS = 1000;
        public const int CARD_SCAN_INTERVAL_MS = 500;

        //Debug Settings
        public const bool ENABLE_DEBUG_OUTPUT = true;
        public const bool SHOW_RAW_DATA = false;
    }
}
