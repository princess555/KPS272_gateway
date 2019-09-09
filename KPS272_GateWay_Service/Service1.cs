using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KPS272_GateWay_Service
{
    public partial class Service1 : ServiceBase
    {
        Parcer parcer;
        private PipeClient pipeClient;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            parcer = new Parcer();
            pipeClient = new PipeClient();

            Thread parcerThread = new Thread(new ThreadStart(parcer.Start));
            parcerThread.Start();

            Thread pipeThread = new Thread(new ThreadStart(parcer.));
        }

        protected override void OnStop()
        {
            parcer.Stop();
            Thread.Sleep(1000);
        }

      
    }
}
