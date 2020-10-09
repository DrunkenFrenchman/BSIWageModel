
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;
using MCM.Abstractions.Settings.Base;



namespace BSIWageModel
{
    public class MySettings : AttributeGlobalSettings<MySettings>
    {
        public override string Id => "BSIWageModel";
        public override string DisplayName => "BSI Wage Model";
        public override string FolderName => "BSIWageModel";
        public override string Format => "json";

        //Main Settings for Wage Model
        [SettingPropertyBool("{=BSIWM_SETTING_08}Wage Model", HintText = "{=BSIWM_SETTING_DESC_12}Check this to enable 'Wage Model'", Order = 0, RequireRestart = true)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}Wage Model", GroupOrder = 0, IsMainToggle = true)]
        public bool BSIWageModelToggle { get; set; } = true;
        [SettingPropertyInteger("{=BSIWM_SETTING_32}Minimum Troop Wage", 1, 25, HintText = "{=BSIWM_SETTING_DESC_32}This number determines the minimum wage paid to a troop. MUST BE SMALLER THAN MAX WAGE MAX", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public int BSIMinWage { get; set; } = 1;
        [SettingPropertyInteger("{=BSIWM_SETTING_01}Maximum Troop Wage", 1, 300, HintText = "{=BSIWM_SETTING_DESC_01}This number determines the maximum wage paid to a troop before mounted and mercenary multipliers are applied. MUST BE LARGER THAN MINIMUM", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public int BSIMaxWage { get; set; } = 60;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_02}Mounted Troop Multiplier", 1f, 10f, HintText = "{=BSIWM_SETTING_DESC_02}This is a multiplier value used to modify the wage of mounted troops", Order = 3, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public float BSIMountedWageMult { get; set; } = 3f;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_31}Mercenary Multiplier", 0.1f, 10f, HintText = "{=BSIWM_SETTING_DESC_31}This is a multiplier value used to modify the wage of mercenary troops", Order = 4, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public float BSIMercenaryWageMult { get; set; } = 1.2f;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_36}Recruitment Cost Multiplier", 1f, 20f, HintText = "{=BSIWM_SETTING_DESC_36}This is a multiplier value used to modify the recruitment cost of troops. Recuitment cost is equal to [daily wage] x [this number]", Order = 5, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public float BSIRecruitmentMult { get; set; } = 10f;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_37}Troop Strength Curve", 0.1f, 3f, HintText = "{=BSIWM_SETTING_DESC_37}Basically, changes shape of curve. Value below one will make lower incomes higher and higher incomes lower. Value above 1 will make higher wages higher and lower wages lower", Order = 6, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_01}Wage Model", GroupOrder = 0)]
        public float BSIStrengthCurve { get; set; } = 1.3f;

        //Main Settings for Food Model
        [SettingPropertyBool("{=BSIWM_SETTING_08}Food Model", HintText = "{=BSIWM_SETTING_DESC_08}NOT YET IMPLEMENTED! Check this to enable 'Food Model'", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}Food Model [NOT YET IMPLEMENTED]", GroupOrder = 1, IsMainToggle = true)]
        public bool BSIFoodModelToggle { get; set; } = false;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_03}Mounted Troop Food Multiplier", 1f, 5f, HintText = "{=BSIWM_SETTING_DESC_03}Food cumsumption multiplier applied to Mounted Units", Order = 1, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_02}Food Model", GroupOrder = 1)]
        public float BSIMountedFoodMult { get; set; } = 2f;
        [SettingPropertyFloatingInteger("{=BSIWM_SETTING_04}Horses in Inventory Food Multiplier", 0f, 2f, HintText = "{=BSIWM_SETTING_DESC_04}Food consumption multiplier applied for each horse in the player's inventory", Order = 2, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_02}Food Model", GroupOrder = 1)]
        public float BSIHorseFoodMult { get; set; } = 0.1f;

        //Mod Compatibilty Options
        [SettingPropertyBool("{=BSIWM_SETTING_BSI}Blood Shit and Iron", HintText = "{=BSIWM_SETTING_DESC_BSI}Check this if you are playing with the Blood Shit and Iron main mod - You really should be...", Order = 0, RequireRestart = true)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}Mod Compatibility Options", GroupOrder = 2)]
        public bool BSIMainModCompat { get; set; } = true;

        //Debugger Toggle
        [SettingPropertyBool("{=BSIWM_SETTING_DEBUG}Wage Model Debug", HintText = "{=BSIWM_SETTING_DESC_DEBUG}Check this to enable Debug mode", Order = 0, RequireRestart = false)]
        [SettingPropertyGroup("{=BSIWM_SETTING_GROUP_03}Debug", GroupOrder = 3, IsMainToggle = true)]
        public bool BSIWMDebug { get; set; } = true;



    }

    //public override IDictionary<string, Func<BaseSettings>> GetAvailablePresets()
    //{
    //    var basePresets = base.GetAvailablePresets(); // include the 'Default' preset that MCM provides
    //    basePresets.Add("Reverse", () => new CustomSettings()
    //    {
    //        Property1 = false,
    //        Property2 = true
    //    });
    //    basePresets.Add("False", () => new CustomSettings()
    //    {
    //        Property1 = false,
    //        Property2 = false
    //    });
    //    basePresets.Add("True", () => new CustomSettings()
    //    {
    //        Property1 = true,
    //        Property2 = true
    //    });
    //    return basePresets;
    //}
}