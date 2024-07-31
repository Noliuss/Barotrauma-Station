using Content.Shared.Examine;
using Content.Shared.Inventory;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;
using Content.Shared.Skill;
using Content.Shared.Skill.Components;

namespace Content.Shared.Skill;

public sealed class ClothingSkillModifierSystem : EntitySystem
{
    [Dependency] private readonly SkillModifierSystem _skillModifiers = default!;

    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ClothingSkillModifierComponent, ComponentGetState>(OnGetState);
        SubscribeLocalEvent<ClothingSkillModifierComponent, ComponentHandleState>(OnHandleState);
        SubscribeLocalEvent<ClothingSkillModifierComponent, InventoryRelayedEvent<RefreshSkillModifiersEvent>>(OnRefreshModifiers);
        SubscribeLocalEvent<ClothingSkillModifierComponent, GetVerbsEvent<ExamineVerb>>(OnClothingVerbExamine);
    }

    // Public API

    public void SetClothingSkillModifierEnabled(EntityUid uid, bool enabled, ClothingSkillModifierComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        if (component.Enabled != enabled)
        {
            component.Enabled = enabled;
            Dirty(component);

            // inventory system will automatically hook into the event raised by this and update accordingly
            if (_container.TryGetContainingContainer(uid, out var container))
            {
                _skillModifiers.RefreshClothingSkillModifiers(container.Owner);
            }
        }
    }

    // Event handlers

    private void OnGetState(EntityUid uid, ClothingSkillModifierComponent component, ref ComponentGetState args)
    {
        args.State = new ClothingSkillModifierComponentState(
            component.HelmModifier,
            component.WeaponsModifier,
            component.MechanicalEngineeringModifier,
            component.ElectricalEngineeringModifier,
            component.MedicalModifier,
            component.Enabled);
    }

    private void OnHandleState(EntityUid uid, ClothingSkillModifierComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not ClothingSkillModifierComponentState state)
            return;

        var diff = component.Enabled != state.Enabled ||
                !MathHelper.CloseTo(component.HelmModifier, state.HelmModifier) ||
                !MathHelper.CloseTo(component.WeaponsModifier, state.WeaponsModifier) ||
                !MathHelper.CloseTo(component.MechanicalEngineeringModifier, state.MechanicalEngineeringModifier) ||
                !MathHelper.CloseTo(component.ElectricalEngineeringModifier, state.ElectricalEngineeringModifier) ||
                !MathHelper.CloseTo(component.MedicalModifier, state.MedicalModifier);

        component.HelmModifier = state.HelmModifier;
        component.WeaponsModifier = state.WeaponsModifier;
        component.MechanicalEngineeringModifier = state.MechanicalEngineeringModifier;
        component.ElectricalEngineeringModifier = state.ElectricalEngineeringModifier;
        component.MedicalModifier = state.MedicalModifier;
        component.Enabled = state.Enabled;

        // Avoid raising the event for the container if nothing changed.
        // We'll still set the values in case they're slightly different but within tolerance.
        if (diff && _container.TryGetContainingContainer(uid, out var container))
        {
            _skillModifiers.RefreshClothingSkillModifiers(container.Owner);
        }
    }

    private void OnRefreshModifiers(EntityUid uid, ClothingSkillModifierComponent component, InventoryRelayedEvent<RefreshSkillModifiersEvent> args)
    {
        if (!component.Enabled)
            return;

        args.Args.ModifySkill(component.HelmModifier,
            component.WeaponsModifier,
            component.MechanicalEngineeringModifier,
            component.ElectricalEngineeringModifier,
            component.MedicalModifier
        );
    }

    private void OnClothingVerbExamine(EntityUid uid, ClothingSkillModifierComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var HelmModifier = component.HelmModifier;
        var WeaponsModifier = component.WeaponsModifier;
        var MechanicalEngineeringModifier = component.MechanicalEngineeringModifier;
        var ElectricalEngineeringModifier = component.ElectricalEngineeringModifier;
        var MedicalModifier = component.MedicalModifier;

        var msg = new FormattedMessage();

        if (HelmModifier != 0){
        if  (HelmModifier > 0){
            msg.AddMarkup(Loc.GetString("clothing-Helm-increase-equal-examine", ("Helm", HelmModifier)));
            }
        else if (HelmModifier < 0)
            msg.AddMarkup(Loc.GetString("clothing-Helm-decrease-equal-examine", ("Helm", HelmModifier)));
        msg.PushNewline();
        }

        if (WeaponsModifier != 0){
        if  (WeaponsModifier > 0)
            msg.AddMarkup(Loc.GetString("clothing-Weapons-increase-equal-examine", ("Weapons", WeaponsModifier)));
        else if (WeaponsModifier < 0)
            msg.AddMarkup(Loc.GetString("clothing-Weapons-decrease-equal-examine", ("Weapons", WeaponsModifier)));
        msg.PushNewline();
        }

        if (MechanicalEngineeringModifier != 0){
        if  (MechanicalEngineeringModifier > 0)
            msg.AddMarkup(Loc.GetString("clothing-MechanicalEngineering-increase-equal-examine", ("MechanicalEngineering", MechanicalEngineeringModifier)));
        else if (MechanicalEngineeringModifier < 0)
            msg.AddMarkup(Loc.GetString("clothing-MechanicalEngineering-decrease-equal-examine", ("MechanicalEngineering", MechanicalEngineeringModifier)));
        msg.PushNewline();
        }

        if (ElectricalEngineeringModifier != 0){
        if  (ElectricalEngineeringModifier > 0)
            msg.AddMarkup(Loc.GetString("clothing-ElectricalEngineering-increase-equal-examine", ("ElectricalEngineering", ElectricalEngineeringModifier)));
        else if (ElectricalEngineeringModifier < 0)
            msg.AddMarkup(Loc.GetString("clothing-ElectricalEngineering-decrease-equal-examine", ("ElectricalEngineering", ElectricalEngineeringModifier)));
        msg.PushNewline();
        }

        if (MedicalModifier != 0){
        if  (MedicalModifier > 0)
            msg.AddMarkup(Loc.GetString("clothing-Medical-increase-equal-examine", ("Medical", MedicalModifier)));
        else if (MedicalModifier < 0)
            msg.AddMarkup(Loc.GetString("clothing-Medical-decrease-equal-examine", ("Medical", MedicalModifier)));
        msg.PushNewline();
        }

        if  (HelmModifier != 0 ||
        WeaponsModifier != 0 ||
        MechanicalEngineeringModifier != 0 ||
        MechanicalEngineeringModifier != 0 ||
        ElectricalEngineeringModifier != 0 ||
        MedicalModifier != 0
        )
        _examine.AddDetailedExamineVerb(args,
            component,
            msg,
            Loc.GetString("clothing-skill-examinable-verb-text"),
            "/Textures/Interface/VerbIcons/outfit.svg.192dpi.png",
            Loc.GetString("clothing-skill-examinable-verb-message")
        );
    }
}
