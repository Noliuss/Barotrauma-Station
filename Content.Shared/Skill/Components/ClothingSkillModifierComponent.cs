using System.Numerics;
using Content.Shared.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization;

namespace Content.Shared.Skill.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(ClothingSkillModifierSystem))]
public sealed partial class ClothingSkillModifierComponent : Component
{
    #region Skill stats

    /// <summary>
    /// Skill Helm of cloth boost
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("HelmModifier")]
    public int HelmModifier = 0;

    /// <summary>
    /// Skill Weapons of cloth boost
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("WeaponsModifier")]
    public int WeaponsModifier = 0;

    /// <summary>
    /// Skill MechanicalEngineering of cloth boost
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("MechanicalEngineeringModifier")]
    public int MechanicalEngineeringModifier = 0;

    /// <summary>
    /// Skill ElectricalEngineering of cloth boost
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("ElectricalEngineeringModifier")]
    public int ElectricalEngineeringModifier = 0;

    /// <summary>
    /// Skill Medical of cloth boost
    /// </summary>
    ///
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("MedicalModifier")]
    public int MedicalModifier = 0;

    /// <summary>
    ///     Is this clothing item currently 'actively' affects you?
    ///     e.g. magboots can be turned on and off.
    ///     e.g. or clothing using something battery which could become dead
    /// </summary>
    [DataField("enabled")] public bool Enabled = true;


    #endregion

}

[Serializable, NetSerializable]
public sealed class ClothingSkillModifierComponentState : ComponentState
{
    public int HelmModifier;
    public int WeaponsModifier;
    public int MechanicalEngineeringModifier;
    public int ElectricalEngineeringModifier;
    public int MedicalModifier;
    public bool Enabled;

    public ClothingSkillModifierComponentState(
        int HelmModifier,
        int WeaponsModifier,
        int MechanicalEngineeringModifier,
        int ElectricalEngineeringModifier,
        int MedicalModifier,
        bool enabled)
    {
        HelmModifier = HelmModifier;
        WeaponsModifier = WeaponsModifier;
        MechanicalEngineeringModifier = MechanicalEngineeringModifier;
        ElectricalEngineeringModifier = ElectricalEngineeringModifier;
        MedicalModifier = MedicalModifier;
        Enabled = enabled;
    }
}
