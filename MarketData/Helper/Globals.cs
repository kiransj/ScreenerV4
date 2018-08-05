using System;

namespace Helper
{
    public class Globals
    {
        public static Logger log = null;
        public static AppOptions options = null;

        private static readonly object padlock = new object();

        static public void InitGlobals(string optionFileName)
        {
            if(log == null && options == null)
            {
                lock(padlock)
                {
                    if(log == null && options == null)
                    {
                        Options.SetOptions(optionFileName);
                        log = Logger.GetLoggerInstance();
                        options = Options.app;
                    }
                }
            }
            else
            {
                throw new Exception("Calling InitGlobals Twice!!");
            }
        }
    }
}