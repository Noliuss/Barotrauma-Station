using Content.Server.GameTicking;
using Content.Server.Skill.Speech.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Content.Shared.Skill.Components;
using Content.Shared.Skill;
using Content.Shared.Skill.Components;
using Content.Server.NPC.Systems;

namespace Content.Server.Skilll.EntitySystems;

public sealed class SkillSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SkillComponent, PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);

        SubscribeLocalEvent<SkillComponent, RefreshSkillModifiersDoAfterEvent>(OnSkillModifiersChanged);

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

        if (skill.TotalIntelligence < 3)
        {
            EntityManager.AddComponent<LowIntelligenceAccentComponent>(uid);
        }
        else
        {
            EntityManager.RemoveComponent<LowIntelligenceAccentComponent>(uid);
        }
    }

    private void setSkill(SkillComponent component,
        SkillPrototype prototype,
        SkillPriority priority)
    {
        switch(prototype.ID)
        {
            case "Strength":
                component.BaseStrength = (int) priority;
                return;
            case "Perception":
                component.BasePerception = (int) priority;
                return;
            case "Endurance":
                component.BaseEndurance = (int) priority;
                return;
            case "Charisma":
                component.BaseCharisma = (int) priority;
                return;
            case "Intelligence":
                component.BaseIntelligence = (int) priority;
                return;
            case "Agility":
                component.BaseAgility = (int) priority;
                return;
            case "Luck":
                component.BaseLuck = (int) priority;
                return;
            default:
                return;
        }
    }

    private void OnSkillModifiersChanged(EntityUid uid, SkillComponent component, RefreshSkillModifiersDoAfterEvent args)
    {
        if (component.TotalIntelligence < 3)
        {
            EnsureComp<LowIntelligenceAccentComponent>(uid);
        }
        else
        {
            EntityManager.RemoveComponent<LowIntelligenceAccentComponent>(uid);
        }

    }
}
