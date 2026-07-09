using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Arcane.ERP;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CreampiePendingComponent : Component
{
    [AutoNetworkedField]
    public NetEntity? Target;
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
