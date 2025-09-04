using NfcAppTrial.Interfaces;
using NfcAppTrial.Enums;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;
using PinMode = NfcAppTrial.Enums.PinMode;
using NfcAppTrial.Constants;

namespace NfcAppTrial.Services
{
    public class LedController
    {
        private readonly IGpioController _gpioController;

        public LedController(IGpioController gpioController)
        {
            _gpioController = gpioController;
            Initialize();
        }

        private void Initialize()
        {
            _gpioController.SetPinMode(PinConfig.GREEN_PIN, PinMode.Output);
            _gpioController.SetPinMode(PinConfig.RED_PIN, PinMode.Output);

            // Turn off both LEDs initially
            TurnOffAll();
        }

        public void TurnOnGreen()
        {
            _gpioController.WritePin(PinConfig.GREEN_PIN, true);
            _gpioController.WritePin(PinConfig.RED_PIN, false);
        }

        public void TurnOnRed()
        {
            _gpioController.WritePin(PinConfig.RED_PIN, true);
            _gpioController.WritePin(PinConfig.GREEN_PIN, false);
        }

        public void TurnOffAll()
        {
            _gpioController.WritePin(PinConfig.GREEN_PIN, false);
            _gpioController.WritePin(PinConfig.RED_PIN, false);
        }

        public void BlinkGreen(int times = 3)
        {
            for (int i = 0; i < times; i++)
            {
                TurnOnGreen();
                System.Threading.Thread.Sleep(200);
                TurnOffAll();
                System.Threading.Thread.Sleep(200);
            }
        }

        public void BlinkRed(int times = 3)
        {
            for (int i = 0; i < times; i++)
            {
                TurnOnRed();
                System.Threading.Thread.Sleep(200);
                TurnOffAll();
                System.Threading.Thread.Sleep(200);
            }
        }
    }
}
