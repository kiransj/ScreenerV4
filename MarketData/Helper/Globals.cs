using System;
using System.IO;

namespace Helper
{
    public class Globals
    {
        public static Logger Log = null;
        public static AppOptions Options = null;

        private static readonly object padlock = new object();

        static public void InitGlobals(string optionFileName)
        {
            if(Log == null && Options == null)
            {
                lock(padlock)
                {
                    if(Log == null && Options == null)
                    {
                        Helper.Options.SetOptions(optionFileName);
                        Log = Logger.GetLoggerInstance();
                        Options = Helper.Options.app;


                        // Create the temp folder if it did not exist
                        if(!Directory.Exists(Options.TmpFolder))
                        {
                            try
                            {
                                Directory.CreateDirectory(Options.TmpFolder);
                            }
                            catch(Exception ex)
                            {
                                Log.Error($"Temp Folder '{Options.TmpFolder}' creation failed due to error '{ex.Message}'\n>>>>>>>>Exiting App with code 1<<<<<<<<<<");
                                Environment.Exit(2);
                            }
                        }

                        // Log contents to a file if required
                        if(Options.LogFileName.Length > 0)
                        {
                            Log.Info($"Logging contents to file {Options.LogFileName}");
                            Log.LogToFile(Options.LogFileName);
                        }

                        Log.Info("Initialized Globals");
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