using System;
using System.Collections.Generic;
using System.Text;

namespace NfcAppTrial.Enums
{

    public enum PinMode
    {
        Input,
        Output
    }

    public enum RfidStatus
    {
        Success,
        Error,
        NoCard,
        AuthenticationFailed
    }

}
