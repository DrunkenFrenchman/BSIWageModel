using System;

using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BSI.WageModel
{
    public class BSIPartyWageModel : PartyWageModel
    {
        
        private static readonly MySettings settings = MySettings.Instance;

        public override int GetTotalWage(MobileParty mobileParty, StatExplainer explanation = null)
        {

            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            foreach (TroopRosterElement member in mobileParty.MemberRoster)
            {
                CharacterObject character = member.Character;
                int getWage = character.TroopWage * member.Number;
                if (character.IsHero)
                {
                    if (character != mobileParty.Party.Owner.CharacterObject)
                        num3 += character.TroopWage;
                    num3 += character.TroopWage;
                }
                else
                {
                    if (character.Tier < 4)
                        num1 += getWage;
                    else if (character.Tier == 4)
                        num2 += getWage;
                    else if (character.Tier > 4)
                        num3 += getWage;
                    if (character.IsInfantry)
                        num4 += member.Number;
                    if (character.IsMounted)
                        num5 += member.Number;
                    if (character.IsArcher)
                    {
                        num6 += member.Number;
                        if (character.Tier >= 4)
                            num7 += member.Number;
                    }
                }
            }
            if (mobileParty.HasPerk(DefaultPerks.Leadership.LevySergeant))
            {
                ExplainedNumber stat = new ExplainedNumber(1f);
                explanation?.AddLine(DefaultPerks.Leadership.LevySergeant.Name.ToString(), (float)(num1 + num2) - (float)(num1 + num2) * stat.ResultNumber);
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.LevySergeant, mobileParty, true, ref stat);
                num1 = MathF.Round(stat.ResultNumber * (float)num1);
                num2 = MathF.Round(stat.ResultNumber * (float)num2);
            }
            if (mobileParty.HasPerk(DefaultPerks.Leadership.VeteransRespect))
            {
                ExplainedNumber stat = new ExplainedNumber(1f);
                explanation?.AddLine(DefaultPerks.Leadership.VeteransRespect.Name.ToString(), (float)(num2 + num3) - (float)(num2 + num3) * stat.ResultNumber);
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Leadership.VeteransRespect, mobileParty, true, ref stat);
                num2 = MathF.Round(stat.ResultNumber * (float)num2);
                num3 = MathF.Round((float)num3 * stat.ResultNumber);
            }
            ExplainedNumber explainedNumber1 = new ExplainedNumber(1f);
            if (mobileParty.IsGarrison && mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsTown)
            {
                PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.MilitaryTradition, mobileParty.CurrentSettlement.Town, ref explainedNumber1);
                PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Berserker, mobileParty.CurrentSettlement.Town, ref explainedNumber1);
                PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.HunterClan, mobileParty.CurrentSettlement.Town, ref explainedNumber1);
                BSIPartyWageModel.CalculatePartialGarrisonWageReduction((float)num4 / (float)mobileParty.MemberRoster.TotalRegulars, mobileParty, DefaultPerks.Polearm.StandardBearer, ref explainedNumber1, true);
                BSIPartyWageModel.CalculatePartialGarrisonWageReduction((float)num5 / (float)mobileParty.MemberRoster.TotalRegulars, mobileParty, DefaultPerks.Riding.CavalryTactics, ref explainedNumber1, true);
                BSIPartyWageModel.CalculatePartialGarrisonWageReduction((float)num6 / (float)mobileParty.MemberRoster.TotalRegulars, mobileParty, DefaultPerks.Crossbow.PeasantLeader, ref explainedNumber1, true);
            }
            ExplainedNumber explainedNumber2 = new ExplainedNumber((float)(num1 + num2 + num3), explanation);
            float num8 = mobileParty.LeaderHero == null || mobileParty.LeaderHero.Clan.Kingdom == null || (mobileParty.LeaderHero.Clan.IsUnderMercenaryService || !mobileParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae)) ? 0.0f : 0.1f;
            if (mobileParty.HasPerk(DefaultPerks.Crossbow.BoltenGuard))
            {
                float num9 = (float)num7 / (float)mobileParty.MemberRoster.TotalRegulars;
                if ((double)num9 > 0.0)
                {
                    float num10 = (float)((double)DefaultPerks.Crossbow.BoltenGuard.PrimaryBonus * (double)num9 * 0.00999999977648258 - 1.0);
                    explainedNumber2.AddFactor(num10, DefaultPerks.Crossbow.BoltenGuard.Name);
                }
            }
            explainedNumber2.AddFactor(num8, DefaultPolicies.MilitaryCoronae.Name);
            explainedNumber2.AddFactor(explainedNumber1.ResultNumber - 1f, new TextObject("{=7BiaPpo2}Perk Effects"));

            return (int)explainedNumber2.ResultNumber;
        }

        private static void CalculatePartialGarrisonWageReduction(
            float troopRatio,
            MobileParty mobileParty,
            PerkObject perk,
            ref ExplainedNumber garrisonWageReductionMultiplier,
            bool isSecondaryEffect)
        {
            if ((double)troopRatio <= 0.0 || mobileParty.CurrentSettlement.Town.Governor == null || !PerkHelper.GetPerkValueForTown(perk, mobileParty.CurrentSettlement.Town))
                return;
            garrisonWageReductionMultiplier.AddFactor(isSecondaryEffect ? (float)((double)perk.SecondaryBonus * (double)troopRatio * 0.00999999977648258) : (float)((double)perk.PrimaryBonus * (double)troopRatio * 0.00999999977648258), perk.Name);
        }

        public override int GetGoldCostForUpgrade(
          PartyBase party,
          CharacterObject characterObject,
          CharacterObject upgradeTarget)
        {
            ExplainedNumber stat = new ExplainedNumber((float)(this.GetTroopRecruitmentCost(upgradeTarget, (Hero)null) - this.GetTroopRecruitmentCost(characterObject, (Hero)null)) / (characterObject.Occupation != Occupation.Mercenary && characterObject.Occupation != Occupation.Gangster ? 2f : 3f));
            if (party.IsMobile && party.LeaderHero != null && party.MobileParty.HasPerk(DefaultPerks.Bow.RenownedArcher, true))
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, party.MobileParty, false, ref stat);
            if (party.IsMobile && party.LeaderHero != null && party.MobileParty.HasPerk(DefaultPerks.Throwing.ThrowingCompetitions))
                PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.ThrowingCompetitions, party.MobileParty, true, ref stat);

            return (int)stat.ResultNumber;
        }

        public override int GetTroopRecruitmentCost(
          CharacterObject troop,
          Hero buyerHero,
          bool withoutItemCost = false
          )
        {

            int baseWage = (int)Math.Round(troop.TroopWage * settings.BSIRecruitmentMult);
            int adjustedWage = baseWage;
            if (buyerHero != null)
            {
                ExplainedNumber explainedNumber = new ExplainedNumber(1f);
                if (troop.Tier >= 2 && buyerHero.GetPerkValue(DefaultPerks.Throwing.HeadHunter))
                    explainedNumber.AddFactor(DefaultPerks.Throwing.HeadHunter.SecondaryBonus * 0.01f);
                if (troop.IsInfantry)
                {
                    if (buyerHero.GetPerkValue(DefaultPerks.OneHanded.ChinkInTheArmor))
                        explainedNumber.AddFactor(DefaultPerks.OneHanded.ChinkInTheArmor.SecondaryBonus * 0.01f);
                    if (buyerHero.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
                        explainedNumber.AddFactor(DefaultPerks.TwoHanded.ShowOfStrength.SecondaryBonus * 0.01f);
                    if (buyerHero.GetPerkValue(DefaultPerks.Polearm.GenerousRations))
                        explainedNumber.AddFactor(DefaultPerks.Polearm.GenerousRations.SecondaryBonus * 0.01f);
                }
                if (troop.IsArcher)
                {
                    if (buyerHero.GetPerkValue(DefaultPerks.Bow.RenownedArcher))
                        explainedNumber.AddFactor(DefaultPerks.Bow.RenownedArcher.SecondaryBonus * 0.01f);
                    if (buyerHero.GetPerkValue(DefaultPerks.Crossbow.Piercer))
                        explainedNumber.AddFactor(DefaultPerks.Crossbow.Piercer.SecondaryBonus * 0.01f);
                }
                adjustedWage = (int)Math.Max(1, Math.Round(baseWage * explainedNumber.ResultNumber));
            }
            return adjustedWage;
        }
    }
}