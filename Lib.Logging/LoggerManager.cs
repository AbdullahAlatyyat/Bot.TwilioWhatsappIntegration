using Serilog;
using System;

namespace Lib.Logging
{
    public class LoggerManager
    {
        private readonly LoggerConfiguration _configuration = new LoggerConfiguration().MinimumLevel.Verbose();
        public LoggerManager()
        {

        }
        public LoggerManager UseFile(string filePath)
        {
            _configuration.WriteTo.Async(c =>
                c.File(filePath, rollingInterval: RollingInterval.Month));
            return this;
        }
        public LoggerManager UseHttp(string uri)
        {
            _configuration.WriteTo.Async(c =>
                c.Http(uri));
            return this;
        }
        public LoggerManager UseConsole()
        {
            _configuration.WriteTo.Async(c =>
                c.Console());
            return this;
        }
        public LoggerManager UseSeq(string serviceUrl)
        {
            _configuration.WriteTo.Async(c =>
                c.Seq(serviceUrl));
            return this;
        }
        public void Apply()
        {
            Log.Logger = _configuration.CreateLogger();
        }
        public void Close()
        {
            Log.CloseAndFlush();
        }
    }
}
