﻿using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using HarmonyLib;
using TaleWorlds.CampaignSystem;

namespace BSIWageModel
{
    public class MySubModule : MBSubModuleBase
    {
        public const string ModId = "BSIWageModel";
        public const string ModName = "BSI Wage Model";
        private static readonly MySettings settings = MySettings.Instance;
        protected override void OnSubModuleLoad()
        {

            System.Diagnostics.Debug.Print("Module Loaded");

            if (settings.BSIWMDebug is true)
            {
                Harmony.DEBUG = true;
                Debugger.DebugStart();
            }
            if (settings.BSIWageModelToggle is true)
            {

                try
                {
                    BSIPatcher.DoWagePatching();
                    BSIWageModel.Debugger.PrintMessage("BSI Wage Model Loaded All Patches"); // Display message on chatlog 
                }
                catch (Exception ex)
                {
                    BSIWageModel.Debugger.PrintMessage("ERROR: BSI Wage Model Patches Failed at Patching!"); // Display message in Game
                    Debugger.AddEntry("HARMONY ERROR: " + ex.Message);
                    Debugger.AddEntry(ex.StackTrace);
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
                        Debugger.AddEntry("BSIPartyWageModel added");
                    }
                    catch (Exception ex)
                    {
                        BSIWageModel.Debugger.PrintMessage("ERROR: BSI Wage Model Failed Initializing!"); // Display message in Game
                        Debugger.AddEntry("CAMPAIGN GAME STARTER ERROR: " + ex.Message);
                        Debugger.AddEntry(ex.StackTrace);
                    }
                }
            }
        }

        public override void OnGameInitializationFinished(Game game)
        {
            Debugger.AddEntry("Starting Data Setup");
            UnitWage.DataSetup(out float min, out float max, out float av);
            UnitWage.weightMin = min;
            UnitWage.weightMax = max;
            UnitWage.weightAv = av;
            
            try { Debugger.AddEntry("Data Setup Complete: Min = " + UnitWage.weightMin + " Max = " + UnitWage.weightMax + " Av = " + UnitWage.weightAv); }

            catch { Debugger.AddEntry("Data Setup Null String Error"); }
        }
    }
}
