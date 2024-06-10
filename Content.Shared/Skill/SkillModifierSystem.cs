using Content.Shared.Inventory;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Content.Shared.DoAfter;
using Content.Shared.Skill.Components;

namespace Content.Shared.Skill
{
    public sealed class SkillModifierSystem : EntitySystem
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;

        public override void Initialize()
        {
            base.Initialize();
//            SubscribeLocalEvent<SkillComponent, ComponentHandleState>(OnHandleState);
        }


//        private void OnHandleState(EntityUid uid, SkillComponent component, ref ComponentHandleState args)
//        {
//            if (args.Current is not SkillModifierComponentState state) return;
//            component.StrengthModifier = state.StrengthModifier;
//            component.PerceptionModifier = state.PerceptionModifier;
//            component.EnduranceModifier = state.EnduranceModifier;
//            component.CharismaModifier = state.CharismaModifier;
//            component.IntelligenceModifier = state.IntelligenceModifier;
//            component.AgilityModifier = state.AgilityModifier;
//            component.LuckModifier = state.LuckModifier;
//        }

        public void RefreshClothingSkillModifiers(EntityUid uid, SkillComponent? skill = null)
        {
            if (!Resolve(uid, ref skill, false))
                return;

            if (_timing.ApplyingState)
                return;

            var ev = new RefreshSkillModifiersEvent();
            RaiseLocalEvent(uid, ev);

            if (ev.HelmModifier == skill.HelmModifier &&
                ev.WeaponsModifier == skill.WeaponsModifier &&
                ev.MechanicalEngineeringModifier == skill.MechanicalEngineeringModifier &&
                ev.ElectricalEngineeringModifier == skill.ElectricalEngineeringModifier &&
                ev.MedicalModifier == skill.MedicalModifier
            )  return;

            skill.HelmModifier = ev.HelmModifier;
            skill.WeaponsModifier = ev.WeaponsModifier;
            skill.MechanicalEngineeringModifier = ev.MechanicalEngineeringModifier;
            skill.ElectricalEngineeringModifier = ev.ElectricalEngineeringModifier;
            skill.MedicalModifier = ev.MedicalModifier;

            var doAfterEventArgs = new DoAfterArgs(EntityManager, uid, TimeSpan.FromSeconds(0), new RefreshSkillModifiersDoAfterEvent(), uid, uid)
//            var doAfterEventArgs = new DoAfterArgs(uid, uid, TimeSpan.FromSeconds(0), new RefreshSkillModifiersDoAfterEvent(), uid, uid);

            {
                BreakOnUserMove = false,
                BreakOnTargetMove = false,
                BreakOnDamage = false,
                NeedHand = false,
                RequireCanInteract = false,
            };
            if (!_doAfter.TryStartDoAfter(doAfterEventArgs))
                return;

            Dirty(skill);
        }


        [Serializable, NetSerializable]
        private sealed class SkillModifierComponentState : ComponentState
        {
            public int HelmModifier;
            public int WeaponsModifier;
            public int MechanicalEngineeringModifier;
            public int ElectricalEngineeringModifier;
            public int MedicalModifier;

        }
    }

    /// <summary>
    ///     Raised on an entity to determine its skill modificators. Any system that wishes to change skill modificators
    ///     should hook into this event and set it then. If you want this event to be raised,
    ///     call <see cref="SkillModifierSystem.RefreshSkillModifiersEvent"/>.
    /// </summary>
    public sealed class RefreshSkillModifiersEvent : EntityEventArgs, IInventoryRelayEvent
    {
        public SlotFlags TargetSlots { get; } = ~SlotFlags.POCKET;
        public int HelmModifier { get; private set; } = 0;
        public int WeaponsModifier { get; private set; } = 0;
        public int MechanicalEngineeringModifier { get; private set; } = 0;
        public int ElectricalEngineeringModifier { get; private set; } = 0;
        public int MedicalModifier { get; private set; } = 0;

        public void ModifySkill(
            int HelmModifier,
            int WeaponsModifier,
            int MechanicalEngineeringModifier,
            int ElectricalEngineeringModifier,
            int MedicalModifier
        )
        {
            HelmModifier += HelmModifier;
            WeaponsModifier += WeaponsModifier;
            MechanicalEngineeringModifier += MechanicalEngineeringModifier;
            ElectricalEngineeringModifier += ElectricalEngineeringModifier;
            MedicalModifier += MedicalModifier;
        }

        // used to modify stats by chems
        public void ModifyHelm(int HelmModifier)
        {
            HelmModifier += HelmModifier;
        }
        public void ModifyWeapons(int WeaponsModifier)
        {
            WeaponsModifier += WeaponsModifier;
        }
        public void ModifyMechanicalEngineering(int MechanicalEngineeringModifier)
        {
            MechanicalEngineeringModifier += MechanicalEngineeringModifier;
        }
        public void ModifyElectricalEngineering(int ElectricalEngineeringModifier)
        {
            ElectricalEngineeringModifier += ElectricalEngineeringModifier;
        }
        public void ModifyMedical(int MedicalModifier)
        {
            MedicalModifier += MedicalModifier;
        }
    }
    [Serializable, NetSerializable]
    public sealed partial class RefreshSkillModifiersDoAfterEvent : SimpleDoAfterEvent
    {
    }
}
