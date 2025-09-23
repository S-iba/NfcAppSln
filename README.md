# NfcAppTrialSln
## --- Connection ---
###  ESP32 -------- RC522

- 3.3V    -----> VCC
- GND     -----> GND
- GPIO22  -----> RST
- GPIO5   -----> SDA/SS
- GPIO23  -----> MOSI
- GPIO19  -----> MISO
- GPIO18  -----> SCK

-----          -----
## Project structure
- RfidReader/
- ├── Program.cs              // Main application 
- ├── RfidCardReader.cs       // RFID card reading logic
- ├── CardDataParser.cs       // Parse and format card data
- ├── Configuration.cs        // Hardware pin configuration
- └── Models/
-    └── CardInfo.cs         // Card information model
