using Serilog;

namespace Microservices.Common.Helpers
{
   public static class GlobalLogger
    {
        public static void ConfigureLog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Seq("http://localhost:5341")
                .MinimumLevel
                .Warning()
                .Enrich
                .FromLogContext()
                .CreateLogger();
        }
    }
}
