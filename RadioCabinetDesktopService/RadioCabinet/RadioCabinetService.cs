using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RadioCabinet
{
    public partial class RadioCabinetService : ServiceBase
    {
        public RadioCabinetService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Console.WriteLine("Service startet");
        }

        protected override void OnStop()
        {
            Console.WriteLine("Service stoppet");
        }
    }
}
