using HarmonyLib;
using System.Reflection;
using TaleWorlds.CampaignSystem;



namespace BSI.WageModel
{
    public static class BSIPatcher
    {
        public static void DoWagePatching()
        {
            Debug.AddEntry("Starting Harmony Patches");

            Harmony harmony = new Harmony("wagemodel.bsi");
            
            //Unit Wage Patch
            MethodInfo original = typeof(CharacterObject).GetProperty("TroopWage").GetGetMethod();
            MethodInfo prefix = typeof(UnitWage).GetMethod("GetTroopWage");
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            
            Debug.AddEntry("Loaded Patch: GetTroopWage");
        }
    }
}
//System.Diagnostics.Debug.Print("harmony patches hit");
