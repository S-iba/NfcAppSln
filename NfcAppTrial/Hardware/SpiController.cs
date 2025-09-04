using NfcAppTrial.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NfcAppTrial.Hardware
{
    public class SpiController : ISpiController
    {
        private bool _isInitialized = false;

        public void Initialize()
        {
            Console.WriteLine("SPI Bus initialized");
            _isInitialized = true;
        }

        public void Write(byte[] data)
        {
            if (!_isInitialized) throw new InvalidOperationException("SPI not initialized");

            Console.WriteLine($"Writing {data.Length} bytes to SPI");
            // Actual SPI write implementation
        }

        public byte[] Read(int length)
        {
            if (!_isInitialized) throw new InvalidOperationException("SPI not initialized");

            Console.WriteLine($"Reading {length} bytes from SPI");
            // Actual SPI read implementation
            return new byte[length];
        }

        public void Dispose()
        {
            Console.WriteLine("SPI Controller disposed");
            _isInitialized = false;
        }
    }
}
