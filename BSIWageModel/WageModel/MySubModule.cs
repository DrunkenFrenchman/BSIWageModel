using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSI.WageModel
{
    public class MySubModule : MBSubModuleBase
    {
        public const string ModId = "BSI.WageModel";
        public const string ModName = "BSI Wage Model";
        private static readonly MySettings settings = MySettings.Instance;
        protected override void OnSubModuleLoad()
        {

            System.Diagnostics.Debug.Print("Module Loaded");

            if (settings.BSIWMDebug is true)
            {
                Harmony.DEBUG = true;
                Debug.DebugStart();
            }
            if (settings.BSIWageModelToggle is true)
            {

                try
                {
                    BSIPatcher.DoWagePatching();
                    BSI.WageModel.Debug.PrintMessage("BSI Wage Model Loaded All Patches"); // Display message on chatlog 
                }
                catch (Exception ex)
                {
                    BSI.WageModel.Debug.PrintMessage("ERROR: BSI Wage Model Patches Failed at Patching!"); // Display message in Game
                    Debug.AddExceptionLog("HARMONY ERROR", ex);
                }
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            Debug.AddEntry("Starting Data Setup");

            UnitWage.DataSetup();

            Debug.AddEntry("Setting Default Party Wage Model Value");
        }
    }
}
