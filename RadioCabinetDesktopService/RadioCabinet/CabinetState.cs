using System;

namespace RadioCabinet
{
    static class CabinetState
    {
        //public enum ButtonState
        //{
        //    LeftSmallWheelHigh = 'a',
        //    LeftSmallWheelLow = 'b',
        //    RightSmallWheelHigh = 'c',
        //    RightSmallWheelLow = 'd',
        //    Button1High = 'e',
        //    Button1Low = 'f',
        //    Button2High = 'g',
        //    Button2Low = 'h',
        //    Button3High = 'i',
        //    Button3Low = 'j',
        //    Button4High = 'k',
        //    Button4Low = 'l',
        //    Button5High = 'm',
        //    Button5Low = 'n',
        //    VolumeWheelStart = 'v'
        //};     
        
        public enum ButtonState
        {
            LeftSmallWheelHigh = 0x01,
            LeftSmallWheelLow = 0x02,
            RightSmallWheelHigh = 0x03,
            RightSmallWheelLow = 0x04,
            Button1High = 0x05,
            Button1Low = 0x06,
            Button2High = 0x07,
            Button2Low = 0x08,
            Button3High = 0x09,
            Button3Low = 0x0A,
            Button4High = 0x0B,
            Button4Low = 0x0C,
            Button5High = 0x0D,
            Button5Low = 0x0E,
            Button6High = 0x0F,
            Button6Low = 0x10,
            LeftBigWheelHigh = 0x11,
            LeftBigWheelLow = 0x12,
            VolumeWheelStart = 0x13
        };


        private static bool _leftSmallWheelHigh;
        private static bool _rightSmallWheelHigh;
        private static bool _button1High;
        private static bool _button2High;
        private static bool _button3High;
        private static bool _button4SHigh;
        private static bool _button5High;
        private static bool _button6High;
        private static bool _leftBigWheelHigh;
        private static bool _volumeWheelStart;

        private static SystemVolumeControl _volumeController  = new SystemVolumeControl();
        private static int _currentVolume;
        private static int _lsbBits;
        private static int _msbBits;
        private static bool _isMSB = true;


        private static void HandleVolumeWheelTurn(int value)
        {
            // First package
            if (_isMSB)
            {
                _msbBits = value << 8;
                _isMSB = false;
            }
            // Second package
            else
            {
                _lsbBits = value;
                _currentVolume = (_msbBits | _lsbBits);

                SetSystemVolume(_currentVolume);

                _isMSB = true;
                _volumeWheelStart = false;
                _msbBits = _lsbBits = 0;
            }
        }

        private static void SetSystemVolume(int volume)
        {
            var newVolume = FlipNumber((int) SystemVolumeControl.MAX_VAL, volume);
            Console.WriteLine("Setting system volume to: " + newVolume);
            try
            {
                _volumeController.SetVolume(newVolume);
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not set system volume: "+e.Message);
            }
        }


        public static int FlipNumber(int max, int value)
        {
            var middle = max/2;
            return (middle - value) + middle;
        }



        public static void HandleCommand(int command)
        {
            if (_volumeWheelStart)
            {
                HandleVolumeWheelTurn(command);
                return;
            }

            var cmd = (ButtonState) command;
            switch (cmd)
            {
                case ButtonState.LeftSmallWheelLow:
                    _leftSmallWheelHigh = false;
                    LeftSmallWheelHandler();
                    break;
                case ButtonState.LeftSmallWheelHigh:
                    _leftSmallWheelHigh = true;
                    LeftSmallWheelHandler();
                    break;

                case ButtonState.RightSmallWheelLow:
                    _rightSmallWheelHigh = false;
                    RightSmallWheelHandler();
                    break;
                case ButtonState.RightSmallWheelHigh:
                    _rightSmallWheelHigh = true;
                    RightSmallWheelHandler();
                    break;
                
                case ButtonState.Button1Low:
                    _button1High = false;
                    Button1Handler();
                    break;
                case ButtonState.Button1High:
                    _button1High = true;
                    Button1Handler();
                    break;
                
                case ButtonState.Button2Low:
                    _button2High = false;
                    Button2Handler();
                    break;
                case ButtonState.Button2High:
                    _button2High = true;
                    Button2Handler();
                    break;

                case ButtonState.Button3Low:
                    _button3High = false;
                    Button3Handler();
                    break;
                case ButtonState.Button3High:
                    _button3High = true;
                    Button3Handler();
                    break;

                case ButtonState.Button4Low:
                    _button4SHigh = false;
                    Button4Handler();
                    break;
                case ButtonState.Button4High:
                    _button4SHigh = true;
                    Button4Handler();
                    break;

                case ButtonState.Button5Low:
                    _button5High = false;
                    Button5Handler();
                    break;
                case ButtonState.Button5High:
                    _button5High = true;
                    Button5Handler();
                    break;

                case ButtonState.Button6Low:
                    _button6High = false;
                    Button6Handler();
                    break;
                case ButtonState.Button6High:
                    _button6High = true;
                    Button6Handler();
                    break;

                case ButtonState.LeftBigWheelLow:
                    _leftBigWheelHigh = false;
                    LeftBigWheelHandler();
                    break;
                case ButtonState.LeftBigWheelHigh:
                    _leftBigWheelHigh = true;
                    LeftBigWheelHandler();
                    break;

                case ButtonState.VolumeWheelStart:
                    _volumeWheelStart = true;
                    break;
                
                default:
                    Console.WriteLine("Unrecognised command: " + command);
                    break;
            }



        }



        // State change handlers
        private static void LeftBigWheelHandler()
        {
            if (_leftBigWheelHigh)
                Console.WriteLine("LeftBigWheel whent high");
            else
                Console.WriteLine("LeftBigWheel whent low");
        }

        private static void LeftSmallWheelHandler()
        {
            if (_leftSmallWheelHigh)
                Console.WriteLine("LeftSmallWheel whent high");
            else
                Console.WriteLine("LeftSmallWheel whent low");
        }

        private static void RightSmallWheelHandler()
        {
            if (_rightSmallWheelHigh)
                Console.WriteLine("RightSmallWheel whent high");
            else
                Console.WriteLine("RightSmallWheel whent low");
        }

        private static void Button1Handler()
        {
            if (_button1High)
                Console.WriteLine("Button1 whent high");
            else
                Console.WriteLine("Button1 whent low");
        }

        private static void Button2Handler()
        {
            if (_button2High)
                Console.WriteLine("Button2 whent high");
            else
                Console.WriteLine("Button2 whent low");
        }

        private static void Button3Handler()
        {
            if (_button3High)
                Console.WriteLine("Button3 whent high");
            else
                Console.WriteLine("Button3 whent low");
        }

        private static void Button4Handler()
        {
            if (_button4SHigh)
                Console.WriteLine("Button4 whent high");
            else
                Console.WriteLine("Button4 whent low");
        }

        private static void Button5Handler()
        {
            if (_button5High)
                Console.WriteLine("Button5 whent high");
            else
                Console.WriteLine("Button5 whent low");
        }

        private static void Button6Handler()
        {
            if (_button6High)
                Console.WriteLine("Button6 whent high");
            else
                Console.WriteLine("Button6 whent low");
        }

    }
}
