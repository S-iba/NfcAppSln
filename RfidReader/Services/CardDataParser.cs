using RfidReader.Constants;
using RfidReader.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Iot.Device.Mfrc522;


namespace RfidReader.Services
{
    public class CardDataParser
    {
        public static CardInfo ParseCard(byte[] uid)
        {
            var cardInfo = new CardInfo()
            {
                Uid = uid,
                ReadTime = DateTime.UtcNow
            };

            if(Configs.ENABLE_DEBUG_OUTPUT)
            {
                Debug.WriteLine($"Card UID detected: {cardInfo.UidString}");
                Debug.WriteLine($"UID length: {uid.Length}");
            }
            return cardInfo;
        }

        public static bool IsValidUid(byte[] uid)
        {
            if(uid == null || uid.Length == 0) return false;

            return uid.Length == 4 || uid.Length == 7 || uid.Length == 10;

        }

        public static string GetUidType(byte[] uid)
        {
            if (uid == null)
            {
                return "Invalid";
            }

            switch (uid.Length)
            {
                case 4:
                    return "Single Size (4 bytes)";
                case 7:
                    return "Double Size (7 bytes)";
                case 10:
                    return "Triple Size (10 bytes)";
                default:
                    return $"Non-standard ({uid.Length} bytes)";
            }
        }

        public static string FormatUidForDisplay(byte[] uid)
        {
            if (uid == null || uid.Length == 0)
                return "No UID";

            var result = string.Empty;
            for (int i = 0; i < uid.Length; i++)
            {
                result += uid[i].ToString("X2");
                if (i < uid.Length - 1)
                    result += " ";
            }
            return result;
        }

        public static uint UidToInteger(byte[] uid)
        {
            if (uid == null || uid.Length != 4) return 0;

            uint result = 0;
            for (int i = 0; i < 4; i++)
            {
                result = (result << 8) | uid[i];
            }
            return result;
        }
    }
}
