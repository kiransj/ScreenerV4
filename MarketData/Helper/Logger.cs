using System;
using System.IO;

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

        public static ref Logger GetLoggerInstance() => ref log;
        public void LogToCosole(bool write) => writeToConsole = write;
        public void LogToFile(string file) {
            // Create the file if it did not exist
            if(!File.Exists(file))
            {
                using(var fileStream = File.Create(file))
                {
                    Filename = file;
                }
            }
            Filename = file;
        }
        public void SetLogLevel(LogLevel level) => logLevel = level;

        public void Debug(string text) => Write($"D-{DateTime.Now}: {text}", LogLevel.DEBUG);
        public void Info(string text) => Write($"I-{DateTime.Now}: {text}", LogLevel.INFO);
        public void Warn(string text) => Write($"W-{DateTime.Now}: {text}", LogLevel.WARN);
        public void Error(string text) => Write($"E-{DateTime.Now}: {text}", LogLevel.ERROR);

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