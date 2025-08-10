using TickerQ.Utilities.Base;
using TickerQ.Utilities.Models;

namespace TickerQExample
{
    public class MyJobs
    {
        private readonly ILogger<MyJobs> _logger;

        public MyJobs(ILogger<MyJobs> logger)
        {
            _logger = logger;
        }

        [TickerFunction("MyCronJob", "*/1 * * * *")]
        public void MyCronJob()
        {
            _logger.LogInformation("Cleaning up logs...");
            // Implement log cleanup logic here
        }

        [TickerFunction("LogCleanUp")]
        public void CleanUpLogs()
        {
            _logger.LogInformation("Cleaning up logs...");
            // Implement log cleanup logic here
        }

        [TickerFunction(functionName: "SendWelcome")]
        public Task SendWelcome(TickerFunctionContext<string> tickerContext, CancellationToken ct)
        {
            Console.WriteLine(tickerContext.Request); // Output: User123
            return Task.CompletedTask;
        }
    }
}
