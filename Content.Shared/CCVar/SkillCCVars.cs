using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

[CVarDefs]
public sealed class SkillCCVars
{

    #region Human editor

    public static readonly CVarDef<int> MaxSkill =
            CVarDef.Create("skill.max", 40);

    #endregion

    #region Strength

    /// <summary>
    ///     Strength to be able to weild anything
    /// </summary>
    public static readonly CVarDef<int> StrengthWeild =
            CVarDef.Create("skillStrength.weild", 3);

    #endregion

    #region Perception
    #endregion

    #region Endurance
    #endregion

    #region Charisma
    #endregion

    #region Intelligence
    #endregion

    #region Agility
    #endregion

    #region Luck
    #endregion

}
