using System;
using System.Collections.Generic;
using System.Text;

namespace NfcAppTrial.Interfaces
{
    public interface ISpiController
    {
        void Initialize();
        void Write(byte[] data);
        byte[] Read(int length);
        void Dispose();
    }
}
