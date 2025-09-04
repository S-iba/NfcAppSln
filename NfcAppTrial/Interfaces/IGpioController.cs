using NfcAppTrial.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NfcAppTrial.Interfaces
{
    public interface IGpioController
    {
        void SetPinMode(int pin, PinMode mode);
        void WritePin(int pin, bool value);
        bool ReadPin(int pin);
        void Initialize();
        void Cleanup();
    }
}
