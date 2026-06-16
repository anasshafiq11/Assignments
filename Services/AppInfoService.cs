using Assignment2.Services.Interfaces;

namespace Assignment2.Services
{
    public class AppInfoService: IAppInfoService
    {
        public string ApplicationId { get; }
        public DateTime StartTime { get; } 

        public AppInfoService()
        {
            ApplicationId = Guid.NewGuid().ToString();
            StartTime = DateTime.Now;

        }
    }
}
