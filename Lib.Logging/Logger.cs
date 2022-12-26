using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Logging
{
    public class Logger
    {
        public Logger()
        {

        }

        public void Debug(string message)
        {
            Log.Logger.Debug(message);
        }
        public void Info(string message)
        {
            Log.Logger.Information(message);
        }
        public void Warning(string message)
        {
            Log.Logger.Warning(message);
        }
        public void Error(string message)
        {
            Log.Logger.Error(message);
        }
        public void Error(Exception exception, string message = "")
        {
            Log.Logger.Error(exception, message);
        }
    }
}
