using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace BSIWageModel
{
    //[HarmonyPatch(typeof(CharacterObject), "TroopWage")]
    public class BSICharacterPatch : BasicCharacterObject
    {
        //[HarmonyPrefix]
        public static bool TroopWage(ref CharacterObject __instance, ref int __result)
        {
            try
            {
                Debugger.AddEntry("Getting Unit Wage for " + __instance.Name.ToString());
                BSIWageModel.UnitWage.GetTroopWage(ref __instance, ref __result);
            }
            catch (Exception ex) { Debugger.AddExceptionLog("UNIT WAGE ERROR", ex); }

            return false;
        }
    }
}
