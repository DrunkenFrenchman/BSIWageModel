using System;
using TaleWorlds.CampaignSystem;
using HarmonyLib;

namespace BSIWageModel
{
    [HarmonyPatch(typeof(CharacterObject), "TroopWage")]
    public class BSITroopWage
    {
        [HarmonyPrefix]
        public static bool TroopWage(CharacterObject __instance, ref int __result)
        {
            __result = BSIWageModel.UnitWage.GetTroopWage(__instance);
            __result = Math.Max(__result, 1);
            return false;
        }
    }
}
