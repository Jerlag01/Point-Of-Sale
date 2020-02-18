using System.ServiceProcess;

namespace Pos.Services.Web
{
    internal static class Program
    {
        private static void Main()
        {
            ServiceBase.Run(new ServiceBase[1]
            {
                (ServiceBase) new WebService()
            });
        }
    }
}