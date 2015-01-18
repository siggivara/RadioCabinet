using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;

namespace RadioCabinet
{



    class RadioCabinetSerialHandler
    {
        // Global variables
        private SerialPort _com;
        public const string COM_PORT = "MSP430 Application UART";

        // Cabinet state

        static public List<String> GetListOfFriendlyComNames()
        {
            List<String> friendlyNames;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                friendlyNames = (from n in portnames join p in ports on n equals p["DeviceID"].ToString() select n + " - " + p["Caption"]).ToList();

            }

            return friendlyNames;
        }


        static public void ListCOMFriendlyNames()
        {
            var friendlyNames = GetListOfFriendlyComNames();

            foreach (string s in friendlyNames)
            {
                Console.WriteLine(s);
            }
        }

        public static string GetComName(string msp430ApplicationUart)
        {

            var comPort = string.Empty;
            var comPortsFriendlyNames = GetListOfFriendlyComNames();

            foreach (var comName in comPortsFriendlyNames)
            {
                if (comName.Contains(COM_PORT))
                {
                    var line = comName.Split(' ');
                    comPort = line[0];
                    break;
                }
            }

            return comPort;
        }


        public void WaitForData()
        {
            var comPort = GetComName(COM_PORT);
            //comPort = "COM12";

            if (string.IsNullOrEmpty(comPort))
            {
                Console.WriteLine("Kunne ikke finne en port med passende navn");
                return;
            }

            

            _com = new SerialPort(comPort, 9600);
            //_com.DataReceived += ComDataReceivedEventHandler;

            try
            {
                _com.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect to " + comPort + ": " + e.Message);
                _com.Close();
                return;
            }
            

            Console.WriteLine(comPort + " ready. Waiting for data");
            while (true)
            {
                try
                {
                    var data = _com.ReadByte();
                    CabinetState.HandleCommand(data);
                }
                catch(Exception e) {Console.WriteLine(e.Message);}
            }
                

            _com.Close();
        }
    }
}
