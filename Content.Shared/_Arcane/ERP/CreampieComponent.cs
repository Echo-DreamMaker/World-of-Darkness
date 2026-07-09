using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._Arcane.ERP;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CreampiePendingComponent : Component
{
    [AutoNetworkedField]
    public NetEntity? Target;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class CreampieInsideComponent : Component
{
    [AutoNetworkedField]
    public int Count;

    [DataField, AutoNetworkedField]
    public float SpeedModifier = 0.95f;

    [DataField]
    public float CumAmount;

    [DataField]
    public float LeakThreshold = 15f;

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan NextLeakAt;
}

[Serializable, NetSerializable]
public enum CreampieConfirmKey : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class CreampieConfirmBuiState(NetEntity user, NetEntity target) : BoundUserInterfaceState
{
    public NetEntity User { get; } = user;
    public NetEntity Target { get; } = target;
}

[Serializable, NetSerializable]
public sealed class CreampieConfirmMessage(bool inside) : BoundUserInterfaceMessage
{
    public readonly bool Inside = inside;
}
