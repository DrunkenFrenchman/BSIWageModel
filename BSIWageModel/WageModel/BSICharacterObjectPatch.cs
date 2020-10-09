using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace BSI.WageModel
{
    //[HarmonyPatch(typeof(CharacterObject), "TroopWage")]
    public class BSICharacterPatch : BasicCharacterObject
    {
        //[HarmonyPrefix]
        public static bool TroopWage(ref CharacterObject __instance, ref int __result)
        {
            try
            {
                Debug.AddEntry("Getting Unit Wage for " + __instance.Name.ToString());
                BSI.WageModel.UnitWage.GetTroopWage(ref __instance, ref __result);
            }
            catch (Exception ex) { Debug.AddExceptionLog("UNIT WAGE ERROR", ex); }

            return false;
        }
    }
}
