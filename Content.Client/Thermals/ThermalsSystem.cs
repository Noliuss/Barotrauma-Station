﻿﻿using Content.Shared.NightVision;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client.NightVision;

public sealed class NightVisionSystem : SharedNightVisionSystem
{
    [Dependency] private readonly ILightManager _light = default!;
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IPlayerManager _player = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NightVisionComponent, LocalPlayerAttachedEvent>(OnNightVisionAttached);
        SubscribeLocalEvent<NightVisionComponent, LocalPlayerDetachedEvent>(OnNightVisionDetached);
    }

    private void OnNightVisionAttached(Entity<NightVisionComponent> ent, ref LocalPlayerAttachedEvent args)
    {
        NightVisionChanged(ent);
    }

    private void OnNightVisionDetached(Entity<NightVisionComponent> ent, ref LocalPlayerDetachedEvent args)
    {
        Off();
    }

    protected override void NightVisionChanged(Entity<NightVisionComponent> ent)
    {
        if (ent != _player.LocalEntity)
            return;

        switch (ent.Comp.State)
        {
            case NightVisionState.Off:
                Off();
                break;
            case NightVisionState.Half:
                Half(ent);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void NightVisionRemoved(Entity<NightVisionComponent> ent)
    {
        if (ent != _player.LocalEntity)
            return;

        Off();
    }

    private void Off()
    {
        _overlay.RemoveOverlay(new NightVisionOverlay());
        _light.DrawLighting = true;
    }

    private void Half(Entity<NightVisionComponent> ent)
    {
        _overlay.AddOverlay(new NightVisionOverlay());
        _light.DrawLighting = true;
    }
}
