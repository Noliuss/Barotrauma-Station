using Content.Shared.Alert;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.NightVision;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
[Access(typeof(SharedNightVisionSystem))]
public sealed partial class NightVisionComponent : Component
{
    [DataField, AutoNetworkedField]
    public NightVisionState State = NightVisionState.Half;

    [DataField, AutoNetworkedField]
    public bool Overlay;

    [DataField, AutoNetworkedField]
    public bool Innate;

    [DataField, AutoNetworkedField]
    public bool SeeThroughContainers;
}

[Serializable, NetSerializable]
public enum NightVisionState
{
    Off,
    Half
}
