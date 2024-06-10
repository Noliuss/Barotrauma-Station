using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

[CVarDefs]
public sealed class SkillCCVars
{

    #region Human editor

    public static readonly CVarDef<int> MaxSkill =
            CVarDef.Create("skill.max", 15);

    #endregion

    #region Helm

    /// <summary>
    ///     Strength to be able to weild anything
    /// </summary>
    // public static readonly CVarDef<int> StrengthWeild =
    //         CVarDef.Create("skillStrength.weild", 3);

    #endregion

    #region Weapons
    #endregion

    #region MechanicalEngineering
    #endregion

    #region ElectricalEngineering
    #endregion

    #region Medical
    #endregion

}
