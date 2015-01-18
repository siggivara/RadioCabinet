using System;
using System.ServiceProcess;

namespace RadioCabinet
{

    static class Program
    {
        


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new RadioCabinetService(),  
            //};
            //ServiceBase.Run(ServicesToRun);


            var radioCabinet = new RadioCabinetSerialHandler();
            radioCabinet.WaitForData();

        }
    }
}
