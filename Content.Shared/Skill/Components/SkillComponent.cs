using System.Numerics;
using Content.Shared.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Skill.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SkillComponent : Component
{
    #region Skill Base Stats

    /// <summary>
    /// Player's skill index
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("helm")]
    public int baseHelm { get; set; } = 5;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("Weapons")]
    public int baseWeapons { get; set; } = 5;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("MechanicalEngineering")]
    public int baseMechanicalEngineering { get; set; } = 5;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("ElectricalEngineering")]
    public int baseElectricalEngineering { get; set; } = 5;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("Medical")]
    public int baseMedical { get; set; } = 5;

    #endregion

    #region Skill job modifiers

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("helmJobModifier")]
    public int helmJobModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("weaponsJobModifier")]
    public int weaponsJobModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("mechanicalEngineeringJobModifier")]
    public int mechanicalEngineeringJobModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("electricalEngineeringJobModifier")]
    public int electricalEngineeringJobModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("medicalJobModifier")]
    public int medicalJobModifier { get; set; } = 0;

    #endregion

    #region Skill modifiers

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("HelmModifier")]
    public int HelmModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("WeaponsModifier")]
    public int WeaponsModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("MechanicalEngineeringModifier")]
    public int MechanicalEngineeringModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("ElectricalEngineeringModifier")]
    public int ElectricalEngineeringModifier { get; set; } = 0;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("MedicalModifier")]
    public int MedicalModifier { get; set; } = 0;

    #endregion

    #region Total Skill

    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalHelm { get { return Math.Clamp(baseHelm + helmJobModifier + HelmModifier, SkillAmountMin, SkillAmountMax); } }

    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalWeapons { get { return Math.Clamp(baseWeapons + weaponsJobModifier + WeaponsModifier, SkillAmountMin, SkillAmountMax); } }

    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalMechanicalEngineering { get { return Math.Clamp(baseMechanicalEngineering + mechanicalEngineeringJobModifier + MechanicalEngineeringModifier, SkillAmountMin, SkillAmountMax); } }

    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalElectricalEngineering { get { return Math.Clamp(baseElectricalEngineering + electricalEngineeringJobModifier + ElectricalEngineeringModifier, SkillAmountMin, SkillAmountMax); } }

    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalMedical { get { return Math.Clamp(baseMedical + medicalJobModifier + MedicalModifier, SkillAmountMin, SkillAmountMax); } }

    #endregion

    #region Skill min/max amount

    /// <summary>
    ///     Don't let Skill go above this value.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public int SkillAmountMax = 200;

    /// <summary>
    ///     Don't let Skill go below this value.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public int SkillAmountMin = 1;

    #endregion

}
