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

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter cgs = gameStarterObject as CampaignGameStarter;
                if (settings.BSIWageModelToggle is true)
                {
                    try
                    {
                        cgs.AddModel(new BSIPartyWageModel());
                        Debug.AddEntry("BSIPartyWageModel added");
                    }
                    catch (Exception ex)
                    {
                        BSI.WageModel.Debug.PrintMessage("ERROR: BSI Wage Model Failed Initializing!"); // Display message in Game
                        Debug.AddExceptionLog("CAMPAIGN GAME STARTER ERROR", ex);
                    }
                }
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            Debug.AddEntry("Starting Data Setup");

            UnitWage.DataSetup();

            Debug.AddEntry("Setting Default Party Wage Model Value");
            try
            {
                typeof(GameModels).GetProperty("PartyWageModel").SetValue(Campaign.Current.Models, new BSIPartyWageModel());
                Debug.AddEntry("Set Party Wage Model to: " + Campaign.Current.Models.PartyWageModel.ToString());
            }
            catch (Exception ex) { Debug.AddExceptionLog("PARTY WAGE MODEL SETTER ERROR", ex); }
        }
    }
}
