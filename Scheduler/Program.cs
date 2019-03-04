using System.ServiceProcess;

namespace Scheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new EmailReminder() 
            };

            DBConnect dbConnect = new DBConnect();
            dbConnect.Select();

            ServiceBase.Run(ServicesToRun);
        }
    }
}
