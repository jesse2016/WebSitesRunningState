using System.ServiceProcess;

namespace GT.SiteCheck.Quartz
{
    static class Program
    {
        static void Main(string[] args)
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //       new JobService()
            //};
            //ServiceBase.Run(ServicesToRun);

            JobPrepare.Run();
        }
    }
}
