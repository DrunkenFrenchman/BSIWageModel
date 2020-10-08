using System;
using TaleWorlds.CampaignSystem;
using HarmonyLib;

namespace BSIWageModel
{
    //[HarmonyPatch(typeof(CharacterObject), "TroopWage")]
    public class BSITroopWage
    {
        //[HarmonyPrefix]
        public static bool TroopWage(ref CharacterObject __instance, ref int __result)
        {
            //DEBUG LOG
            Debugger.AddEntry("TroopWage Hit");
            //DEBUG LOG

            try
            {
                Debugger.AddEntry("Getting Unit Wage for " + __instance.Name.ToString());
                BSIWageModel.UnitWage.GetTroopWage(ref __instance, ref __result);
            }
            catch (Exception ex) { Debugger.AddExceptionLog("UNIT WAGE ERROR", ex); }
            __result = 1000;

            //DEBUG LOG
            Debugger.AddEntry("TroopWage End");
            //DEBUG LOG

            return false;
        }
    }
}
