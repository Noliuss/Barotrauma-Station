using Content.Server.GameTicking;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Content.Shared.Skill.Components;
using Content.Shared.Skill;
using Content.Shared.Skill.Components;
using Content.Server.NPC.Systems;

namespace Content.Server.Skill.EntitySystems;

public sealed class SkillSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkillComponent, PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);

//        SubscribeLocalEvent<SkillComponent, RefreshSkillModifiersDoAfterEvent>(OnSkillModifiersChanged);

    }

    // When the player is spawned in, add all trait components selected during character creation
    private void OnPlayerSpawnComplete(EntityUid uid, SkillComponent component, PlayerSpawnCompleteEvent args)
    {
        if (!EntityManager.TryGetComponent<SkillComponent>(uid, out var skill))
        {
            return;
        }

        foreach (var item in args.Profile.SkillPriorities)
        {
            if (!_prototypeManager.TryIndex<SkillPrototype>(item.Key, out var skillPrototype))
            {
                Logger.Warning($"No skill prototype found with ID {item.Key}!");
                return;
            }
            setSkill(skill, skillPrototype, item.Value);
        }
    }

    private void setSkill(SkillComponent component,
        SkillPrototype prototype,
        SkillPriority priority)
    {
        switch(prototype.ID)
        {
            case "Helm":
                component.baseHelm += (int) priority;
                return;
            case "Weapons":
                component.baseWeapons += (int) priority;
                return;
            case "MechanicalEngineering":
                component.baseMechanicalEngineering += (int) priority;
                return;
            case "ElectricalEngineering":
                component.baseElectricalEngineering += (int) priority;
                return;
            case "Medical":
                component.baseMedical += (int) priority;
                return;
            default:
                return;
        }
    }
}
