using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace BSIWageModel
{
    public class UnitWage
    {
        private static readonly MySettings settings = MySettings.Instance;

        public static float weightMin, weightMax, weightAv;
        public static void DataSetup(out float min, out float max, out float av)
        {
            float[] weightFork = new float[2] { -1, -1 };
            List<float> unitScores = new List<float>(GatherData(weightFork));
            min = unitScores.Min();
            max = unitScores.Max();
            av = unitScores.Average();
        }

        private static List<float> GatherData(float[] fork)
        {
            //DEBUG LOG
            Debugger.AddEntry("GatherData Hit");
            //DEBUG LOG

            float weightMin = fork[0];
            float weightMax = fork[1];
            List<float> initUnitScores = new List<float>();

            foreach (CharacterObject co in CharacterObject.All)
            {
                if (co != null && !co.IsTemplate && (co.Occupation == Occupation.Soldier || co.Occupation == Occupation.Mercenary || co.Occupation == Occupation.Bandit || co.Occupation == Occupation.Gangster || co.Occupation == Occupation.CaravanGuard))
                {

                    co.GetSimulationAttackPower(out float attack_points, out float defense_points);
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
            }

            //DEBUG LOG
            Debugger.AddEntry("GatherData End");
            //DEBUG LOG

            return initUnitScores;
        }
        private static float GetWeightFactor(CharacterObject __instance)
        {
            __instance.GetSimulationAttackPower(out float attack_points, out float defense_points);
            float weight = attack_points + defense_points;
            float min = weightMin;
            float max = weightMax;
            float av = weightAv;
            float factor = (float)Math.Pow(((weight - min) / (max - min)) - (min / (max - min)), 2);
            return factor;
        }

        private static float GetTypeFactor(CharacterObject __instance)
        {
            float mercMult = (__instance.Occupation == Occupation.Mercenary || __instance.Occupation == Occupation.Gangster) ? settings.BSIMercenaryWageMult : 1;
            float mountedMult = __instance.HasMount() ? settings.BSIMountedWageMult : 1;

            return mercMult * mountedMult;
        }

        public static bool GetTroopWage(ref CharacterObject __instance, ref int __result)
        {
            //DEBUG LOG
            Debugger.AddEntry("GetTroopWage Hit");
            //DEBUG LOG

            int min = settings.BSIMinWage;
            int max = settings.BSIMaxWage;
            if (!__instance.IsHero)
            {

                __result = (int)Math.Round(min + (GetTypeFactor(__instance) * (GetWeightFactor(__instance) * (max - min))));
                __result = Math.Max(__result, 1);
            }

            else
            {
                __result = (int)Math.Round(min + (GetTypeFactor(__instance) * (float)Math.Pow((double)(__instance.Level / 32), 2) * (max - min)));
                __result = Math.Max(__result, 1);
            }

            //DEBUG LOG
            Debugger.AddEntry("GetTroopWage End");
            //DEBUG LOG

            return false;
        }
    }
}