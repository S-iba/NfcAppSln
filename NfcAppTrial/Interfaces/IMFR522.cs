using System;
using System.Collections.Generic;
using System.Text;

namespace NfcAppTrial.Interfaces
{
    public interface IMFRC522
    {
        void PCD_Init();
        bool PICC_IsNewCardPresent();
        bool PICC_ReadCardSerial();
        byte[] GetCardUid();
        void PCD_Reset();
        void Dispose();
    }
}
