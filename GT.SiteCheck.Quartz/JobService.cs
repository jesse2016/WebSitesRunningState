using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GT.SiteCheck.Quartz
{
    public partial class JobService : ServiceBase
    {
        public JobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //JobStatePrepare job = new JobStatePrepare();
            //job.JobRun();

            //ServiceTest.start();

            JobPrepare.Run();
        }

        protected override void OnStop()
        {
            JobPrepare.Shut();
        }
    }
}
