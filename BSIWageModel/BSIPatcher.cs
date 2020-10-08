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
            Debugger.AddEntry("Starting Harmony Patches");

            Harmony harmony = new Harmony("wagemodel.bsi");
            
            //Unit Wage Patch
            MethodInfo original = typeof(CharacterObject).GetProperty("TroopWage").GetGetMethod();
            MethodInfo prefix = typeof(UnitWage).GetMethod("GetTroopWage");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            
            Debugger.AddEntry("Loaded Patch: GetTroopWage");
        }
    }
}
//System.Diagnostics.Debug.Print("harmony patches hit");
