using System;
using System.IO;
using System.Threading;

namespace Helper
{
    public enum LogLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3
    }
    public class Logger
    {
        private static readonly object fileLock = new object();
        private static readonly object consoleLock = new object();
        private static Logger log = new Logger();
        private LogLevel logLevel;
        private string Filename = null;
        private bool writeToConsole = true;


        private string TimeNow()
        {
            return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public static ref Logger GetLoggerInstance() => ref log;
        public void LogToCosole(bool write) => writeToConsole = write;
        public void LogToFile(string file) {
            // Create the file if it did not exist
            if(!File.Exists(file))
            {
                try {
                    using(var fileStream = File.Create(file))
                    {
                        Filename = file;
                    }
                }
                catch(Exception ex)
                {
                    Error($"Unable to create log file '{file}' due to '{ex.Message}'.\n>>>>>>>Exiting with code 1<<<<<<<");
                    Environment.Exit(1);
                }
            }
            Filename = file;
        }

        public void SetLogLevel(LogLevel level) => logLevel = level;
        public void Debug(string text) => Write($"{TimeNow()},{Thread.CurrentThread.ManagedThreadId} DEBUG {text}", LogLevel.DEBUG);
        public void Info(string text) => Write($"{TimeNow()},{Thread.CurrentThread.ManagedThreadId} INFO {text}", LogLevel.INFO);
        public void Warn(string text) => Write($"{TimeNow()},{Thread.CurrentThread.ManagedThreadId} WARN {text}", LogLevel.WARN);
        public void Error(string text) => Write($"{TimeNow()},{Thread.CurrentThread.ManagedThreadId} ERROR {text}", LogLevel.ERROR);

        private Logger()
        {
             logLevel = LogLevel.DEBUG;
        }

        private void Write(string text, LogLevel level)
        {
            if(level >= logLevel)
            {
                if(writeToConsole)
                {
                    lock(consoleLock)
                    {
                        if(level == LogLevel.ERROR)  Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", text);
                        Console.ResetColor();
                    }
                }
                if(Filename != null)
                {
                    lock(fileLock)
                    {
                        File.AppendAllText(Filename, $"{text}\n");
                    }
                }
            }
        }
    }
}