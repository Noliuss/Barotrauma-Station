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

            if (ev.StrengthModifier == skill.StrengthModifier &&
                ev.PerceptionModifier == skill.PerceptionModifier &&
                ev.EnduranceModifier == skill.EnduranceModifier &&
                ev.CharismaModifier == skill.CharismaModifier &&
                ev.IntelligenceModifier == skill.IntelligenceModifier &&
                ev.AgilityModifier == skill.AgilityModifier &&
                ev.LuckModifier == skill.LuckModifier
            )  return;

            skill.StrengthModifier = ev.StrengthModifier;
            skill.PerceptionModifier = ev.PerceptionModifier;
            skill.EnduranceModifier = ev.EnduranceModifier;
            skill.CharismaModifier = ev.CharismaModifier;
            skill.IntelligenceModifier = ev.IntelligenceModifier;
            skill.AgilityModifier = ev.AgilityModifier;
            skill.LuckModifier = ev.LuckModifier;

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
            public int StrengthModifier;
            public int PerceptionModifier;
            public int EnduranceModifier;
            public int CharismaModifier;
            public int IntelligenceModifier;
            public int AgilityModifier;
            public int LuckModifier;

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
        public int StrengthModifier { get; private set; } = 0;
        public int PerceptionModifier { get; private set; } = 0;
        public int EnduranceModifier { get; private set; } = 0;
        public int CharismaModifier { get; private set; } = 0;
        public int IntelligenceModifier { get; private set; } = 0;
        public int AgilityModifier { get; private set; } = 0;
        public int LuckModifier { get; private set; } = 0;

        public void ModifySkill(
            int strengthModifier,
            int perceptionModifier,
            int enduranceModifier,
            int charismaModifier,
            int intelligenceModifier,
            int agilityModifier,
            int luckModifier
        )
        {
            StrengthModifier += strengthModifier;
            PerceptionModifier += perceptionModifier;
            EnduranceModifier += enduranceModifier;
            CharismaModifier += charismaModifier;
            IntelligenceModifier += intelligenceModifier;
            AgilityModifier += agilityModifier;
            LuckModifier += luckModifier;
        }

        // used to modify stats by chems
        public void ModifyStrength(int strengthModifier)
        {
            StrengthModifier += strengthModifier;
        }
        public void ModifyPerception(int perceptionModifier)
        {
            PerceptionModifier += perceptionModifier;
        }
        public void ModifyEndurance(int enduranceModifier)
        {
            EnduranceModifier += enduranceModifier;
        }
        public void ModifyCharisma(int charismaModifier)
        {
            CharismaModifier += charismaModifier;
        }
        public void ModifyIntelligence(int intelligenceModifier)
        {
            IntelligenceModifier += intelligenceModifier;
        }
        public void ModifyAgility(int agilityModifier)
        {
            AgilityModifier += agilityModifier;
        }
        public void ModifyLuck(int luckModifier)
        {
            LuckModifier += luckModifier;
        }
    }
    [Serializable, NetSerializable]
    public sealed partial class RefreshSkillModifiersDoAfterEvent : SimpleDoAfterEvent
    {
    }
}
