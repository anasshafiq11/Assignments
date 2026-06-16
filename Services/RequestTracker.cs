using Assignment2.Services.Interfaces;

namespace Assignment2.Services
{
    public class RequestTracker: IRequestTracker
    {
        public string RequestId { get; }

        public RequestTracker()
        {
            RequestId = Guid.NewGuid().ToString();
        }

    }
}
