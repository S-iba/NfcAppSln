# NfcAppTrialSln

A .NET nanoFramework application for reading RFID/NFC tags using the MFRC522 RFID module and ESP32.

## Hardware Connection
###  ESP32 -------- RC522

- 3.3V    -----> VCC
- GND     -----> GND
- GPIO22  -----> RST
- GPIO5   -----> SDA/SS
- GPIO23  -----> MOSI
- GPIO19  -----> MISO
- GPIO18  -----> SCK

## Overview

This application provides a robust interface for reading RFID/NFC tags using GPIO controls on an ESP32 microcontroller. It implements continuous tag reading in a non-blocking manner using background threading.

## Features

- Continuous RFID/NFC tag reading
- Non-blocking operation using background thread
- Hardware abstraction using GPIO controller
- Real-time tag ID output in hexadecimal format
- Built on .NET nanoFramework for embedded systems
- ESP32 compatible

## Requirements

### Hardware
- ESP32 microcontroller
- MFRC522 RFID module
- Appropriate wiring/connections as shown in the connection diagram above

### Software
- .NET nanoFramework
- System.Device.Gpio package
- Visual Studio with nanoFramework support

## Project Structure
- RfidReader/
  - `Program.cs` - Main application entry point and threading logic
  - `Sensors/RfidReaderSensor.cs` - RFID sensor abstraction layer

## How It Works

1. The application initializes GPIO pins for communication with the MFRC522 module
2. A background thread is created to handle continuous RFID reading
3. When a tag is detected, its unique ID is read and output
4. The reading process repeats every second
5. The main thread keeps the application running indefinitely

## Implementation Details

The program uses a two-thread architecture:
- Main thread: Handles initialization and keeps the application alive
- Reader thread: Continuously polls for RFID tags without blocking the main thread

## Output Format

Tag IDs are output through the debug console in hexadecimal format:
```
04-25-AF-CD-7A
```

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/S-iba/NfcAppSln
   ```
2. Connect your ESP32 and MFRC522 module following the connection diagram
3. Open the solution in Visual Studio
4. Build and deploy to your ESP32

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Create a new Pull Request

## Repository

This project is hosted at: https://github.com/S-iba/NfcAppSln

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
