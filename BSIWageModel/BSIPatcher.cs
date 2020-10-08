using HarmonyLib;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;



namespace BSIWageModel
{
    public static class BSIPatcher
    {
        public static void DoWagePatching()
        {
            MySettings settings = MySettings.Instance;
            if (settings.BSIWMDebug is true) { Debugger.AddEntry("Starting Harmony Patches"); }

            var harmony = new Harmony("wagemodel.bsi");
            
            //TroopWage Patch
            harmony.Patch((MethodBase)typeof(CharacterObject).GetMethod("TroopWage"), new HarmonyMethod(typeof(BSITroopWage).GetMethod("TroopWage")));
            Debugger.AddEntry("Loaded Patch: TroopWage");
        }
    }
}
//System.Diagnostics.Debug.Print("harmony patches hit");
