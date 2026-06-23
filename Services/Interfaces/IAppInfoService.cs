namespace Assignment2.Services.Interfaces
{
    // This interface defines a contract for retrieving application information such as a unique application ID and the start time of the application.
    public interface IAppInfoService
    {
        string ApplicationId { get; }
        DateTime StartTime { get; }
    }
}
