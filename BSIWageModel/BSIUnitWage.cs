using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BSIWageModel
{
    public class UnitWage
    {
        private static readonly MySettings settings = MySettings.Instance;
        public static List<CharacterObject> unitList = new List<CharacterObject>();
        public static float weightMin, weightMax;
        public static float valueMin, valueMax;
        public static void DataSetup()
        {
            GatherValidCharacters();
            GatherWeightData();
            GatherEquipmentData();
        }

        private static void GatherWeightData()
        {
            //DEBUG LOG
            Debugger.AddEntry("Gather Weight Data Hit");
            //DEBUG LOG            
           
            //Run Weight Calculations on Relevant Characters
 
            List<float> initUnitScores = new List<float>();
            initUnitScores.Clear();

            foreach (CharacterObject co in unitList)
            {
                co.GetSimulationAttackPower(out float attack_points, out float defense_points, co.Equipment);
                float weight = (attack_points + defense_points);

                if (weightMin != -1 && weight < weightMin)
                {
                    weightMin = weight;
                }
                if (weightMax != -1 && weight > weightMax)
                {
                    weightMax = weight;
                }
                else
                {
                    weightMin = weight;
                    weightMax = weight;
                }

                initUnitScores.Add(weight);
            }

            weightMin = initUnitScores.Min();
            weightMax = initUnitScores.Max();

            //DEBUG LOG
            Debugger.AddEntry("Gather Weight Data End || Min =" + weightMin + " || Max =" + weightMax);
            //DEBUG LOG
        }

        //Build List of Valid Chars
        public static void GatherValidCharacters()
        {
            unitList.Clear();
            Debugger.AddEntry("Gathering Valid Characters with BSI Compat Option set to " + settings.BSIMainModCompat.ToString());
            foreach (CharacterObject characterObject in CharacterObject.All)
            {
                if (settings.BSIMainModCompat is true && characterObject.StringId.StartsWith("mod_"))
                {
                    unitList.Add(characterObject);
                }
                else if (characterObject != null && !characterObject.IsTemplate && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Bandit || characterObject.Occupation == Occupation.Gangster || characterObject.Occupation == Occupation.CaravanGuard))
                {
                    unitList.Add(characterObject);
                }
            }

            //User Error Exception Catcher
            if (unitList.IsEmpty())
            {
                Debugger.AddEntry("Method Found no valid troops! Please make sure are not using Main Mod Compat option incorrectly!");
                Debugger.PrintMessage("ERROR: Please make sure only use BSI Wage Model Compatiblity Options if needed!");
                settings.BSIMainModCompat = false;
                try { GatherValidCharacters(); }
                catch (Exception ex) { Debugger.AddExceptionLog("GATHER VALID CHARACTERS ERROR", ex); }
            }
            Debugger.AddEntry("Detected " + unitList.Count().ToString() + " relevant units");
        }

        //Build List for Min and Max equipment value
        public static void GatherEquipmentData()
        {
            Debugger.AddEntry("Gather Equipment Data Hit");

            List<float> initEquipmentScores = new List<float>();
            foreach (CharacterObject co in unitList)
            {
                float total = 0;
                foreach (Equipment e in co.BattleEquipments)
                {
                    for (int i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
                    {
                        ItemObject item = e[i].Item;
                        if (item != null)
                            total += (float)Math.Sqrt(item.Value);
                    }
                }
                total = total * 10 / co.BattleEquipments.Count();

                initEquipmentScores.Add(total);
            }

            valueMin = initEquipmentScores.Min();
            valueMax = initEquipmentScores.Max();

            Debugger.AddEntry("Gather Equipment Data End || Min =" + valueMin + " || Max =" + valueMax);
        }

        private static float GetWeightFactor(CharacterObject __instance)
        {
            __instance.GetSimulationAttackPower(out float attack_points, out float defense_points);

            float weight = attack_points + defense_points + GetCashValue(__instance);
            float min = weightMin + valueMin;
            float max = weightMax + valueMax;
            float factor = (float)Math.Pow(((weight - min) / (max - min)) - (min / (max - min)), settings.BSIStrengthCurve);
            return factor;
        }

        private static float GetCashValue(CharacterObject __instance)
        {

            float total = 0;
            foreach (Equipment e in __instance.BattleEquipments)
            {
                for (int i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
                {
                    ItemObject item = e[i].Item;
                    if (item != null)
                        total += (float) Math.Sqrt(item.Value);
                }
            }
            total = total / __instance.BattleEquipments.Count();

            return total;
        }

        private static float GetTypeFactor(CharacterObject __instance)
        {
            float mercMult = (__instance.Occupation == Occupation.Mercenary || __instance.Occupation == Occupation.Gangster) ? settings.BSIMercenaryWageMult : 1;
            float mountedMult = __instance.HasMount() ? settings.BSIMountedWageMult : 1;

            return mercMult * mountedMult;
        }

        public static bool GetTroopWage(ref CharacterObject __instance, ref int __result)
        {

            int min = settings.BSIMinWage;
            int max = settings.BSIMaxWage;
            if (!__instance.IsHero)
            {

                __result = (int)Math.Round(min + (GetTypeFactor(__instance) * (GetWeightFactor(__instance) * (max - min) / (6 / Math.Max(1, __instance.Tier)))));
                __result = Math.Max(__result, 1);
            }

            else
            {
                __result = (int)Math.Round(min + (GetTypeFactor(__instance) * (float)Math.Pow((double)(__instance.Level / 32), settings.BSIStrengthCurve) * (max - min)));
                __result = Math.Max(__result, 1);
            }

            return false;
        }
    }
}