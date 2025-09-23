
using RfidReader.Sensors;
using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace RfidReader
{
    public class Program
    {
        static RfidReaderSensor _rfIdReader;

        static byte[] _lastReadTag;

        public static void Main()
        {

            GpioController gpioController = new GpioController();

            _rfIdReader = new RfidReaderSensor(gpioController);

            _rfIdReader.Initialize();

            ThreadStart threadStart = new ThreadStart(RfidWork);
            Thread thread = new Thread(threadStart);
            thread.Start();


            if(_lastReadTag!=null)
                


            Thread.Sleep(Timeout.Infinite);
        }


        public static void RfidWork()
        {
            while (true)
            {
                var id = _rfIdReader.ReadCardNfcId();
                _lastReadTag = id;
                Debug.WriteLine(BitConverter.ToString(id));

                Thread.Sleep(1000);
            }

        }
    }
    }

  
