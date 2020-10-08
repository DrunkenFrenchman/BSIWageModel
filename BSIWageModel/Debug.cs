using System;
using System.IO;
using TaleWorlds.Core;
using System.Xml;
using System.Net;

namespace BSIWageModel
{

    public class Debugger
    {
        private static readonly MySettings settings = MySettings.Instance;
        private static string pathString;
        private static string fileName = "bsi_wagemodel.debug.log";

        //Print Message in Game Helper
        public static void PrintMessage(string message) 
        {
            if (message != null) { InformationManager.DisplayMessage(new InformationMessage(message)); }
            else { InformationManager.DisplayMessage(new InformationMessage("BSI Wage Model tried printing null message!")); }
        }

        //Return Date and Time helper
        public static string DateTime() 
        {
            return System.DateTime.Now.ToString();
        }

        //Log File Direectory Helper
        public static string GetDirectory() 
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = System.IO.Path.Combine(filePath, "Mount and Blade II Bannerlord");
            filePath = System.IO.Path.Combine(filePath, "Logs");
            return filePath;
        }

        //Add Log Helper
        public static void AddEntry(string entry) 
        {
            if (settings.BSIWMDebug is false) { return; }

            //Create Directory
            string path = GetDirectory();
            if (System.IO.Directory.Exists(path) != true)
            {
                DebugStart();
            }
            if (entry == "New Session Start: Module Loaded") { entry = "\n\n\n" + DateTime() + "==>" + entry; }
            else { entry = "\n" + DateTime() + "==>" + entry; }

            System.IO.File.AppendAllText(Path.Combine(GetDirectory(), fileName), entry);

        }

        //Initialize Debug
        public static void DebugStart() 
        {

            if (!System.IO.Directory.Exists(GetDirectory()))
            {
                System.IO.Directory.CreateDirectory(GetDirectory());
                BSIWageModel.Debugger.PrintMessage("BSI Wage Model Debug File Path Created");
                AddEntry("Log Folder Created");
            }
          
            AddEntry("New Session Start: Module Loaded");
        }
    }
}
