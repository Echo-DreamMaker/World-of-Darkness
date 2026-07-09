using Robust.Shared.GameStates;

namespace Content.Shared._Arcane.ERP;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class CreampieInsideComponent : Component
{
    [AutoNetworkedField]
    public int Count;

    [AutoNetworkedField]
    public TimeSpan NextLeakAt;

    [AutoNetworkedField]
    public float SpeedModifier = 0.95f;
}
