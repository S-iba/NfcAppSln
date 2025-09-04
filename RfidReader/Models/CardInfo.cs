using System;
using System.Collections.Generic;
using System.Text;

namespace RfidReader.Models
{
    public class CardInfo
    {
        public byte[] Uid { get; set; }

        public string UidString => ConvertToHexString(Uid);

        public string UidDecimal => ConvertToDecimalString(Uid);

        public DateTime ReadTime { get; set; }

        public CardInfo() 
        {
            ReadTime = DateTime.UtcNow;
        }

        private string ConvertToHexString(byte[] data)
        {
           if (data == null || data.Length == 0)
           {
               return string.Empty;
            }

           var result = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i].ToString("X2");
                if (i < data.Length - 1)
                {
                    result += ":";
                }
            }
            return result;
        }

        private string ConvertToDecimalString(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            if (data.Length <= 4)
            {
                uint decimalValue = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    decimalValue = (decimalValue << 8) | data[i];
                }
                return decimalValue.ToString();
            }
            else
            {
                var result = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    result += data[i].ToString();
                    if (i < data.Length - 1)
                    {
                        result += ":";
                    }
                }
                return result;
            }
        }

        public override string ToString()
        {
            return $"UID: {UidString} ({UidDecimal}) - Read: {ReadTime:HH:mm:ss}";
        }
    }
}
